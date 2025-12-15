// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Spatial.Compute;
using Spatial.Networking.Contracts;
using Spatial.Networking.Contracts.Miscellaneous;
using Spatial.Structures;
using System.Buffers;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection;

namespace Spatial.Networking;

/// <summary>
/// A high-performance asynchronous private network.
/// </summary>
public partial class Network
{
    private readonly IServiceProvider _services;
    private readonly Dictionary<Type, Controller> _controllers;
    private readonly Dictionary<ushort, (Controller Controller, Command Command, Type Prototype)> _operations;

    private readonly List<Socket> _endpoints;
    private readonly ConcurrentDictionary<long, Connection> _connections;
    private readonly InterlockedQueue<NetworkEvent> _events;
    private readonly InterlockedQueue<Message> _updates;

    /// <summary>
    /// Create a new <see cref="Network"/>.
    /// </summary>
    public Network(IServiceProvider services)
    {
        _services = services;
        _controllers = [];
        _operations = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(asm => asm.GetTypes().Where(type => type.IsAssignableTo(typeof(Controller))))
            .SelectMany(type => type.GetMethods().Where(method => method.GetCustomAttribute<Controller.OperationAttribute>(true) != null))
            .ToDictionary(
                keySelector: method => method.GetCustomAttribute<Controller.OperationAttribute>(true)!.Code,
                elementSelector: method => {
                    var controller = GetOrCreateController(method.DeclaringType!);
                    var prototype = method.GetParameters()[0].ParameterType;

                    var type = typeof(Action<>).MakeGenericType(prototype);
                    var delg = Delegate.CreateDelegate(type, controller, method);

                    return (controller, (Command) ((data) => delg.DynamicInvoke(data)), prototype);
                });

        _endpoints = [];
        _connections = [];
        _events = new();
        _updates = new();
    }

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
    public void Listen(string endpoint)
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        socket.Bind(IPEndPoint.Parse(endpoint));
        socket.Listen();

        BeginAccept(socket);

        _endpoints.Add(socket);
    }

    /// <summary>
    /// Close the <see cref="Network"/>.
    /// </summary>
    public void Close()
    {
        foreach (var endpoint in _endpoints)
        {
            endpoint.Close();
        }

        _connections.Clear();
        _events.Clear();
        _updates.Clear();
        _operations.Clear();
    }

    /// <summary>
    /// Connect a <see cref="Socket"/> to the <see cref="Network"/>.
    /// </summary>
    /// <param name="socket">A <see cref="Socket"/>.</param>
    /// <param name="bridge">An optional <see cref="Bridge"/>.</param>
    internal Connection Connect(Socket socket, Bridge? bridge = default)
    {
        socket.NoDelay = true;

        return Connection.Allocate(this, socket, bridge).Connect();
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
                    Log.Verbose("Delivering {size} KB packet to {connection}.", Math.Round(packet.Count / 1024D, 2), connection.Id);
                    
                    try
                    {
                        if (connection.Bridge is not null)
                        {
                            connection.Bridge?.Socket.Web.SendAsync(packet, WebSocketMessageType.Binary, true, CancellationToken.None).GetAwaiter().GetResult();
                        }
                        else
                        {
                            connection.Socket.Send(packet);
                        }
                    }
                    catch (SocketException exception) when (exception.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        connection.Disconnect();
                    }
                }

                ArrayPool<byte>.Shared.Return(packet.Array!);
            }
        }
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
                Connect(socket);
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
                    Log.Verbose("Client {connection} connected to the server.", connection);

                    _connections[connection.Id] = connection;

                    Send(
                        connection: connection,
                        command: (ushort) NETCOMMAND.NC_MISC_SEED_CMD,
                        data: new PROTO_NC_MISC_SEED_CMD(connection.Seed),
                        encrypt: false);

                    break;
                }
            case NetworkEventCode.EVENT_DISCONNECT:
                {
                    Log.Verbose("Client {connection} disconnected from the server.", connection);

                    _connections.TryRemove(connection.Id, out _);

                    connection.Dispose();

                    break;
                }
            case NetworkEventCode.EVENT_MESSAGE:
                {
                    var message = e.Message!;

                    if (_operations.TryGetValue(message.Command, out var command))
                    {
                        Log.Verbose("Invoking operation 0x{command:X4} on behalf of {connection}.", message.Command, connection.Id);

                        try
                        {
                            using var data = ProtocolBuffer.FromSpan(command.Prototype, message.Data.AsSpan(2, message.Size - 2));

                            command.Controller.Connection = connection;
                            command.Controller.Message = message;

                            command.Command(data);
                        }
                        catch (Exception exception)
                        {
                            Log.Error(exception, "Failed to invoke operation 0x{command:X4} on behalf of {connection}.", message.Command, connection.Id);
                        }
                        finally
                        {
                            ArrayPool<byte>.Shared.Return(message.Data);
                        }
                    }

                    message.Dispose();
                    break;
                }
        }
    }

    private Controller GetOrCreateController(Type type)
    {
        if (!_controllers.TryGetValue(type, out var controller))
        {
            controller = _controllers[type] = (Controller) ActivatorUtilities.CreateInstance(_services, type)!;
        }

        return controller;
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

        Log.Verbose("Issuing 0x{command:X4} to {connection}.", command, connection.Id);

        _updates.Enqueue(Message.Create(connection, command, buffer, size));
    }

    /// <summary>
    /// Multicast a <see cref="NETCOMMAND"/> on behalf of the <see cref="Network"/>.
    /// </summary>
    /// <param name="command">A <see cref="NETCOMMAND"/>.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="filter">An optional filter.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/>.</param>
    public void Multicast(ushort command, ProtocolBuffer data, Expression<Func<Connection, bool>>? filter = default, bool dispose = true)
    {
        data.Serialize(true);

        var func = filter?.Compile();

        Job.ParallelFor(_connections, (_, connection) =>
        {
            if (func == default || func(connection))
            {
                Send(connection, command, data, false, false);
            }
        });

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
    public void Broadcast(ushort command, ProtocolBuffer data, bool dispose = true)
    {
        Multicast(command, data, default, dispose);
    }
}