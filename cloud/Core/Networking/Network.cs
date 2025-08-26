// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Spatial.Compute.Jobs;
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
public partial class Network
{
    private readonly IServiceProvider _services;
    private readonly Socket _socket;
    private readonly Dictionary<Type, Controller> _controllers;
    private readonly Dictionary<ushort, (Controller Controller, Command Handler, Type Prototype)> _handlers;
    private int _open;

    private readonly ConcurrentDictionary<long, Connection> _connections;
    private readonly InterlockedQueue<NetworkEvent> _events;
    private readonly InterlockedQueue<Message> _updates;

    /// <summary>
    /// Create a new <see cref="Network"/>.
    /// </summary>
    public Network(IServiceProvider services)
    {
        _services = services;
        _socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _controllers = [];
        _handlers = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(asm => asm.GetTypes().Where(type => type.IsAssignableTo(typeof(Controller))))
            .SelectMany(type => type.GetMethods().Where(method => method.GetCustomAttribute<HandlerAttribute>(true) != null))
            .ToDictionary(
                keySelector: method => method.GetCustomAttribute<HandlerAttribute>(true)!.Command,
                elementSelector: method => 
                {
                    var controller = GetOrCreateController(method.DeclaringType!);
                    var prototype = method.GetParameters()[0].ParameterType;

                    var type = typeof(Action<>).MakeGenericType(prototype);
                    var delg = Delegate.CreateDelegate(type, controller, method);

                    return (controller, (Command) ((data) => delg.DynamicInvoke(data)), prototype);
                });

        _connections = [];
        _events = new();
        _updates = new();
    }

    /// <summary>
    /// The network's endpoint.
    /// </summary>
    public IPEndPoint Endpoint => _socket.LocalEndPoint as IPEndPoint ?? throw new InvalidOperationException("The server has not been opened.");

    /// <summary>
    /// Active connections to the <see cref="Network"/>.
    /// </summary>
    public ConcurrentDictionary<long, Connection> Connections => _connections;

    /// <summary>
    /// The network's event queue.
    /// </summary>
    internal InterlockedQueue<NetworkEvent> Queue => _events;

    /// <summary>
    /// Open the <see cref="Network"/>.
    /// </summary>
    /// <param name="endpoint">The server's <see cref="IPEndPoint"/>.</param>
    public void Open(IPEndPoint endpoint)
    {
        if (Interlocked.Exchange(ref _open, 1) != 0)
        {
            return;
        }

        _socket.Bind(endpoint);
        _socket.Listen();

        BeginAccept();

        Log.Information("Server opened at {endpoint}.", Endpoint);
    }

    /// <summary>
    /// Close the <see cref="Network"/>.
    /// </summary>
    public void Close()
    {
        if (Interlocked.Exchange(ref _open, 0) != 1)
        {
            return;
        }

        _socket.Close();
        _connections.Clear();
        _events.Clear();
        _updates.Clear();
        _handlers.Clear();
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
                        connection.Socket.Send(packet.AsSpan(0, packet.Count));
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

    private void BeginAccept()
    {
        if (Interlocked.CompareExchange(ref _open, 0, 0) == 0)
        {
            return;
        }

        _socket.BeginAccept(EndAccept, null);
    }

    private void EndAccept(IAsyncResult e)
    {
        try
        {
            if (_socket.EndAccept(e) is Socket socket)
            {
                Connect(socket);
            }
        }
        catch (Exception exception)
        {
            ERROR(exception, "Activation of network connection failed.");
        }
        finally
        {
            BeginAccept();
        }
    }

    private void Connect(Socket socket)
    {
        socket.NoDelay = true;

        Connection.Allocate(this, socket).Connect();
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

                    connection.Command(
                        command: NETCOMMAND.NC_MISC_SEED_CMD,
                        data: new PROTO_NC_MISC_SEED_CMD(connection.Seed));

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

                    if (!_handlers.TryGetValue(message.Command, out var command))
                    {
                        Log.Verbose("Unhandled 0x{command:X4} from {connection}.", message.Command, connection.Id);
                    }
                    else
                    {
                        Log.Verbose("Handling 0x{command:X4} from {connection}.", message.Command, connection.Id);

                        try
                        {
                            using var data = ProtocolBuffer.FromSpan(command.Prototype, message.Data.AsSpan(2, message.Size - 2));

                            command.Controller.Connection = connection;
                            command.Controller.Message = message;

                            command.Handler(data);
                        }
                        catch (Exception exception)
                        {
                            Log.Error(exception, "Failed to handle 0x{command:X4} from {connection}.", message.Command, connection.Id);
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
    /// Issue a <see cref="NETCOMMAND"/> on behalf of the <see cref="Network"/>.
    /// </summary>
    /// <param name="connection">A <see cref="Connection"/>.</param>
    /// <param name="command">A <see cref="NETCOMMAND"/>.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/>.</param>
    /// <param name="serialize">Whether or not to serialize the <see cref="ProtocolBuffer"/>.</param>
    public void Command(Connection connection, ushort command, ProtocolBuffer data, bool dispose = true, bool serialize = true)
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
                Command(connection, command, data, false, false);
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