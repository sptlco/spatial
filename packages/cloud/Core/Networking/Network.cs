// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using Spatial.Networking.Contracts;
using Spatial.Networking.Contracts.Miscellaneous;
using Spatial.Structures;
using System.Buffers;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Spatial.Networking;

/// <summary>
/// A high-performance asynchronous private network.
/// </summary>
public partial class Network : IDisposable
{
    private static Transformer _transformer = new NativeTransformer();

    private readonly Dictionary<Socket, object?> _contexts;
    private readonly Dictionary<Type, Controller> _controllers;
    private readonly Dictionary<ushort, (Controller Controller, Delegate Command, Type Prototype)> _operations;
    private readonly List<Socket> _endpoints;
    private readonly ConcurrentDictionary<long, Connection> _connections;
    private readonly InterlockedQueue<NetworkEvent> _events;
    private readonly InterlockedQueue<Message> _updates;

    /// <summary>
    /// Create a new <see cref="Network"/>.
    /// </summary>
    public Network()
    {
        _contexts = [];
        _controllers = [];
        _operations = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(asm => asm.GetTypes().Where(type => type.IsAssignableTo(typeof(Controller))))
            .SelectMany(type => type.GetMethods().Where(method => method.GetCustomAttribute<Controller.OperationAttribute>(true) != null))
            .ToDictionary(
                keySelector: method => method.GetCustomAttribute<Controller.OperationAttribute>(true)!.Code,
                elementSelector: method => {
                    var controller = GetOrCreateController(method.DeclaringType!);
                    var prototype = method.GetParameters()[1].ParameterType;

                    if (method.ReturnType == typeof(Task))
                    {
                        return (controller, (Delegate) CreateAsyncCommand(controller, method, prototype), prototype);
                    }

                    return (controller, CreateCommand(controller, method, prototype), prototype);
                });

        _endpoints = [];
        _connections = [];
        _events = new();
        _updates = new();
    }

    /// <summary>
    /// The network's <see cref="Transformer"/>.
    /// </summary>
    public static Transformer Transformer
    {
        get => _transformer;
        set => _transformer = value;
    }

    /// <summary>
    /// Optional resolver that returns a human-readable name for an operation code.
    /// Set this before using the network. If the resolver returns null, the code is formatted as hex.
    /// </summary>
    public static Func<ushort, string?>? OperationNameResolver { get; set; }

    /// <summary>
    /// The network's public endpoints.
    /// </summary>
    public List<Socket> Endpoints => _endpoints;

    /// <summary>
    /// Active connections to the <see cref="Network"/>.
    /// </summary>
    public ConcurrentDictionary<long, Connection> Connections => _connections;

    /// <summary>
    /// The network's event queue.
    /// </summary>
    internal InterlockedQueue<NetworkEvent> Queue => _events;

    /// <summary>
    /// Listen for connections at an <paramref name="endpoint"/>.
    /// </summary>
    /// <param name="endpoint">The endpoint to listen at.</param>
    /// <param name="context">Optional context.</param>
    /// <param name="description">A message describing what the endpoint is used for.</param>
    public void Listen(string endpoint, object? context = default, string? description = default)
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var ip = IPEndPoint.Parse(endpoint);

        socket.Bind(ip);
        socket.Listen();

        _contexts[socket] = context;

        BeginAccept(socket);

        _endpoints.Add(socket);

        var binding = (IPEndPoint) socket.LocalEndPoint!;

        if (string.IsNullOrEmpty(description))
        {
            INFO("Listener bound on port {Port}", binding.Port);
        }
        else
        {
            INFO("Listener bound on port {Port} for {Description}", binding.Port, description);
        }
    }

    /// <summary>
    /// Open an outgoing <see cref="Connection"/> to a remote endpoint.
    /// </summary>
    public Connection Connect(IPEndPoint endpoint, object? context = default)
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        socket.Connect(endpoint);
        socket.NoDelay = true;
        socket.SendTimeout = Constants.SendTimeout;

        var connection = Connection.Allocate(this, socket).AsOutbound();

        connection.Context = context;

        return connection.Connect();
    }

    /// <summary>
    /// Connect a <see cref="Socket"/> to the <see cref="Network"/>.
    /// </summary>
    /// <param name="socket">A <see cref="Socket"/>.</param>
    /// <param name="endpoint">The listener this <see cref="Socket"/> was accepted on.</param>
    internal Connection Connect(Socket socket, Socket endpoint)
    {
        socket.NoDelay = true;
        socket.SendTimeout = Constants.SendTimeout;

        var connection = Connection.Allocate(this, socket).Connect();

        connection.Context = _contexts.GetValueOrDefault(endpoint);

        return connection;
    }

    /// <summary>
    /// Receive data from connections to the <see cref="Network"/>.
    /// </summary>
    public void Receive()
    {
        while (_events.TryDequeue(out var e))
        {
            Process(e);
            e.Dispose();
        }
    }

    /// <summary>
    /// Send data to <see cref="Network"/> connections.
    /// </summary>
    public void Send()
    {
        var packets = new Dictionary<Connection, List<ArraySegment<byte>>>();

        while (_updates.TryDequeue(out var update))
        {
            if (!packets.TryGetValue(update.Connection, out var payload))
            {
                payload = packets[update.Connection] = [];
            }

            if (payload.Count == 0 || payload[^1].Count + update.Size > Constants.ConnectionSize)
            {
                var buffer = ArrayPool<byte>.Shared.Rent(Constants.ConnectionSize);
                var segment = new ArraySegment<byte>(buffer, 0, 0);

                payload.Add(segment);
            }

            var packet = payload[^1];

            Buffer.BlockCopy(update.Data, 0, packet.Array!, packet.Count, update.Size);
            ArrayPool<byte>.Shared.Return(update.Data);

            payload[^1] = new ArraySegment<byte>(
                array: packet.Array!,
                offset: 0,
                count: packet.Count + update.Size);

            TRACE("Connection {Connection}: queued {Command}", update.Connection.ShortUID, Resolve(update.Command));

            update.Dispose();
        }

        foreach (var (connection, payload) in packets)
        {
            foreach (var packet in payload)
            {
                if (packet.Count <= 0)
                {
                    continue;
                }

                if (connection.Connected)
                {
                    TRACE("Connection {Connection}: delivering {Size} KB.", connection.ShortUID, Math.Round(packet.Count / 1024D, 2));

                    try
                    {
                        connection.Socket.Send(packet);
                    }
                    catch (SocketException exception) when (exception.SocketErrorCode is SocketError.ConnectionReset or SocketError.TimedOut)
                    {
                        connection.Disconnect();
                    }
                }

                ArrayPool<byte>.Shared.Return(packet.Array!);
            }
        }
    }

    /// <summary>
    /// Dispose of the <see cref="Network"/>.
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);

        foreach (var endpoint in _endpoints)
        {
            endpoint.Dispose();
        }

        while (_events.TryDequeue(out var e))
        {
            if (e.Message?.Data is not null)
            {
                ArrayPool<byte>.Shared.Return(e.Message.Data);
            }
        }

        _connections.Clear();
        _events.Clear();
        _updates.Clear();
        _operations.Clear();
    }

    private void BeginAccept(Socket endpoint)
    {
        endpoint.BeginAccept(EndAccept, endpoint);
    }

    private void EndAccept(IAsyncResult e)
    {
        var endpoint = (Socket) e.AsyncState!;

        try
        {
            if (endpoint.EndAccept(e) is Socket socket)
            {
                Connect(socket, endpoint);
            }

            BeginAccept((Socket) e.AsyncState!);
        }
        catch (Exception) { }
    }

    private void Process(in NetworkEvent e)
    {
        var connection = e.Connection;

        switch (e.Code)
        {
            case NetworkEventCode.EVENT_CONNECT:
                {
                    _connections[connection.Id] = connection;

                    if (connection.Outbound)
                    {
                        INFO("Connection {Connection}: established outbound to {Endpoint}", connection.ShortUID, connection.IP);
                    }
                    else
                    {
                        INFO("Connection {Connection}: accepted inbound from {Endpoint}", connection.ShortUID, connection.IP);
                        TRACE("Connection {Connection}: issuing seed 0x{Seed:X4}", connection.ShortUID, connection.Seed);

                        Send(
                            connection: connection,
                            command: (ushort) NETCOMMAND.NC_MISC_SEED_CMD,
                            data: new PROTO_NC_MISC_SEED_CMD(connection.Seed),
                            encrypt: false);
                    }

                    break;
                }
            case NetworkEventCode.EVENT_DISCONNECT:
                {
                    INFO("Connection {Connection}: closed ({Direction})", connection.ShortUID, connection.Outbound ? "outbound" : "inbound");

                    _connections.TryRemove(connection.Id, out _);

                    connection.DisconnectImpl(this);
                    connection.Dispose();

                    break;
                }
            case NetworkEventCode.EVENT_MESSAGE:
                {
                    using var message = e.Message!;
                    var bytes = message.Data;

                    try
                    {
                        if (message.Command == (ushort) NETCOMMAND.NC_MISC_SEED_CMD)
                        {
                            var seed = ProtocolBuffer.FromSpan<PROTO_NC_MISC_SEED_CMD>(bytes.AsSpan(2, message.Size - 2)).Seed;

                            connection.SetSeed(seed);

                            TRACE("Connection {Connection}: seed received 0x{Seed:X4}", connection.ShortUID, seed);
                        }
                        else
                        {
                            Transformer.Decode(bytes, 2, message.Size - 2, connection.Keys);
                        }

                        if (!_operations.TryGetValue(message.Command, out var operation))
                        {
                            ERROR("Connection {Connection}: {Command} is not implemented.", connection.ShortUID, Resolve(message.Command));
                            break;
                        }

                        TRACE("Connection {Connection}: {Operation}", connection.ShortUID, Resolve(message.Command));

                        var data = ProtocolBuffer.FromSpan(operation.Prototype, bytes.AsSpan(2, message.Size - 2));

                        switch (operation.Command)
                        {
                            case AsyncCommand task:
                                Task.Run(async () => {
                                    try
                                    {
                                        await task(connection, data);
                                    }
                                    catch (Exception exception)
                                    {
                                        Report(exception, message, connection);
                                    }
                                    finally
                                    {
                                        data.Dispose();
                                        ArrayPool<byte>.Shared.Return(bytes);
                                    }
                                });

                                break;
                            case Command command:
                                try
                                {
                                    command(connection, data);
                                }
                                catch (Exception exception)
                                {
                                    Report(exception, message, connection);
                                }
                                finally
                                {
                                    data.Dispose();
                                    ArrayPool<byte>.Shared.Return(bytes);
                                }

                                break;
                        }
                    }
                    catch
                    {
                        ArrayPool<byte>.Shared.Return(bytes);
                        throw;
                    }

                    break;
                }
        }
    }

    private Controller GetOrCreateController(Type type)
    {
        if (!_controllers.TryGetValue(type, out var controller))
        {
            controller = _controllers[type] = (Controller) ActivatorUtilities.CreateInstance(Application.Current.Services, type)!;
        }

        return controller;
    }

    private void Report(Exception exception, Message message, Connection connection)
    {
        ERROR(exception, "Connection {Connection}: {Operation} failed.", connection.ShortUID, Resolve(message.Command));
    }

    private static string Resolve(ushort command)
    {
        var name = OperationNameResolver?.Invoke(command);

        if (!string.IsNullOrEmpty(name))
        {
            return name!;
        }

        return $"0x{command:X4}";
    }

    private static Command CreateCommand(Controller controller, MethodInfo method, Type prototype)
    {
        var connectionParam = Expression.Parameter(typeof(Connection), "connection");
        var dataParam = Expression.Parameter(typeof(ProtocolBuffer), "data");

        var call = Expression.Call(
            instance: Expression.Constant(controller),
            method: method,
            arg0: connectionParam,
            arg1: Expression.Convert(dataParam, prototype));

        return Expression.Lambda<Command>(call, connectionParam, dataParam).Compile();
    }

    private static AsyncCommand CreateAsyncCommand(Controller controller, MethodInfo method, Type prototype)
    {
        var connectionParam = Expression.Parameter(typeof(Connection), "connection");
        var dataParam = Expression.Parameter(typeof(ProtocolBuffer), "data");

        var call = Expression.Call(
            instance: Expression.Constant(controller),
            method: method,
            arg0: connectionParam,
            arg1: Expression.Convert(dataParam, prototype));

        return Expression.Lambda<AsyncCommand>(call, connectionParam, dataParam).Compile();
    }
}

public partial class Network
{
    /// <summary>
    /// Send a message to a <see cref="Connection"/>.
    /// </summary>
    /// <param name="connection">A <see cref="Connection"/>.</param>
    /// <param name="command">A <see cref="NETCOMMAND"/>.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/>.</param>
    /// <param name="serialize">Whether or not to serialize the <see cref="ProtocolBuffer"/>.</param>
    /// <param name="encrypt">Whether or not to encrypt the message.</param>
    public void Send(Connection connection, ushort command, ProtocolBuffer data, bool dispose = true, bool serialize = true, bool encrypt = true)
    {
        if (serialize)
        {
            data.Serialize(true);
        }

        var array = data.ToArray();
        var size = array.Length + sizeof(ushort);

        if (dispose)
        {
            data.Dispose();
        }

        if (encrypt)
        {
            connection.Encrypt(array);
        }

        byte[] buffer;

        if (size <= byte.MaxValue)
        {
            buffer = ArrayPool<byte>.Shared.Rent(size + 1);
            buffer[0] = (byte) size;

            Buffer.BlockCopy(BitConverter.GetBytes(command), 0, buffer, 1, sizeof(ushort));
            Buffer.BlockCopy(array, 0, buffer, 3, array.Length);

            size += 1;
        }
        else
        {
            buffer = ArrayPool<byte>.Shared.Rent(size + 3);
            buffer[0] = 0;

            Buffer.BlockCopy(BitConverter.GetBytes((ushort) size), 0, buffer, 1, sizeof(ushort));
            Buffer.BlockCopy(BitConverter.GetBytes(command), 0, buffer, 3, sizeof(ushort));
            Buffer.BlockCopy(array, 0, buffer, 5, array.Length);

            size += 3;
        }

        _updates.Enqueue(Message.Create(connection, command, buffer, size));
    }

    /// <summary>
    /// Multicast a <see cref="NETCOMMAND"/> on behalf of the <see cref="Network"/>.
    /// </summary>
    /// <param name="command">A <see cref="NETCOMMAND"/>.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="filter">An optional filter.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/>.</param>
    /// <param name="encrypt">Whether or not to encrypt the <see cref="ProtocolBuffer"/>.</param>
    public void Multicast(ushort command, ProtocolBuffer data, Func<Connection, bool>? filter = default, bool dispose = true, bool encrypt = true)
    {
        data.Serialize(true);

        foreach (var (_, connection) in _connections)
        {
            if (filter == default || filter(connection))
            {
                Send(connection, command, data, false, false, encrypt);
            }
        }

        if (dispose)
        {
            data.Dispose();
        }
    }

    /// <summary>
    /// Broadcast a <see cref="NETCOMMAND"/> on behalf of the <see cref="Network"/>.
    /// </summary>
    /// <param name="command">A <see cref="NETCOMMAND"/>.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/>.</param>
    /// <param name="encrypt">Whether or not to encrypt the <see cref="ProtocolBuffer"/>.</param>
    public void Broadcast(ushort command, ProtocolBuffer data, bool dispose = true, bool encrypt = true)
    {
        Multicast(command, data, default, dispose, encrypt);
    }
}