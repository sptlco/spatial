// Copyright © Spatial. All rights reserved.

using Serilog;
using Spatial.Mathematics;
using System.Buffers;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace Spatial.Networking;

/// <summary>
/// An active connection to the <see cref="Server"/>.
/// </summary>
public sealed class Connection : IDisposable
{
    private static readonly ConcurrentBag<Connection> _pool = [];
    private static long _counter;

    private Socket _socket;
    private int _connected;
    private byte[] _buffer;
    private ushort _seed;
    private readonly Dictionary<string, object?> _metadata;

    /// <summary>
    /// Create a new <see cref="Connection"/>.
    /// </summary>
    public Connection()
    {
        _metadata = [];
    }

    /// <summary>
    /// The connection's identification number.
    /// </summary>
    public long Id { get; } = Interlocked.Increment(ref _counter);
    
    /// <summary>
    /// Whether or not the <see cref="Connection"/> is connected 
    /// to the <see cref="Server"/>.
    /// </summary>
    public bool Connected => Interlocked.CompareExchange(ref _connected, 1, 1) == 1;

    /// <summary>
    /// The connection's <see cref="Socket"/>.
    /// </summary>
    internal Socket Socket => _socket;

    /// <summary>
    /// The connection's current seed.
    /// </summary>
    public ushort Seed => _seed;

    /// <summary>
    /// Properties attached to the <see cref="Connection"/>.
    /// </summary>
    public Dictionary<string, object?> Metadata => _metadata;

    /// <summary>
    /// Allocate a <see cref="Connection"/>.
    /// </summary>
    /// <param name="socket">A <see cref="Socket"/>.</param>
    /// <returns>A <see cref="Connection"/>.</returns>
    internal static Connection Allocate(Socket socket)
    {
        if (!_pool.TryTake(out var connection))
        {
            connection = new();
        }

        connection._socket = socket;
        connection._buffer = ArrayPool<byte>.Shared.Rent(Constants.ConnectionSize);

        return connection;
    }

    /// <summary>
    /// Connect the <see cref="Connection"/> to a <see cref="Server"/>.
    /// </summary>
    internal void Connect()
    {
        _seed = Strong.UInt16((ushort) Constants.ServerBits.Length);
        _connected = 1;

        Server.Queue.Enqueue(NetworkEvent.Create(this, NetworkEventCode.EVENT_CONNECT, null));

        BeginReceive();
    }

    /// <summary>
    /// Disconnect the <see cref="Connection"/>.
    /// </summary>
    public void Disconnect()
    {
        if (Interlocked.CompareExchange(ref _connected, 0, 1) == 1)
        {
            Server.Queue.Enqueue(NetworkEvent.Create(this, NetworkEventCode.EVENT_DISCONNECT));
        }
    }

    /// <summary>
    /// Get a <see cref="Connection"/> property.
    /// </summary>
    /// <typeparam name="T">The type of property to get.</typeparam>
    /// <param name="property">The name of the property.</param>
    /// <returns>The property's value.</returns>
    public T? Get<T>(string property)
    {
        if (!_metadata.TryGetValue(property, out var value))
        {
            return default;
        }

        return (T?) value;
    }

    /// <summary>
    /// Project a <see cref="Connection"/> property.
    /// </summary>
    /// <typeparam name="TSource">The property's type.</typeparam>
    /// <typeparam name="TTarget">The target type.</typeparam>
    /// <param name="property">The name of the property.</param>
    /// <param name="projection">A projection that converts the property value into the target type.</param>
    /// <returns>The projected property value.</returns>
    public TTarget Project<TSource, TTarget>(string property, Func<TSource?, TTarget> projection)
    {
        return projection(Get<TSource>(property));
    }

    /// <summary>
    /// Set a <see cref="Connection"/> property value.
    /// </summary>
    /// <typeparam name="T">The type of property to set.</typeparam>
    /// <param name="property">The name of the property.</param>
    /// <param name="value">The property's value.</param>
    public void Set<T>(string property, T? value)
    {
        _metadata[property] = value;
    }

    /// <summary>
    /// Issue a command to the <see cref="Connection"/>.
    /// </summary>
    /// <param name="command">The command to issue.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/>.</param>
    public void Command(ushort command, ProtocolBuffer data, bool dispose = true)
    {
        if (!Connected)
        {
            return;
        }
        
        Server.Command(this, command, data, dispose);
    }

    /// <summary>
    /// Dispose of the <see cref="Connection"/>.
    /// </summary>
    public void Dispose()
    {
        ArrayPool<byte>.Shared.Return(_buffer);

        _socket.Dispose();
        _pool.Add(this);
        _metadata.Clear();
    }

    private void BeginReceive()
    {
        if (!Connected)
        {
            return;
        }
        
        _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, EndReceive, null);
    }

    private void EndReceive(IAsyncResult e)
    {
        if (!Connected)
        {
            return;
        }

        try
        {
            var offset = 0;
            var bytes = _socket.EndReceive(e);

            if (bytes <= 0)
            {
                Disconnect();
                return;
            }

            Log.Verbose("Received {size} KB packet from {connection}.", Math.Round(bytes / 1024D, 2), Id);

            while (bytes > 0)
            {
                var size = (ushort) _buffer[offset];

                offset += 1;
                bytes -= 1;

                if (size == 0)
                {
                    size = BitConverter.ToUInt16(_buffer, offset);

                    offset += sizeof(ushort);
                    bytes -= sizeof(ushort);
                }

                var buffer = ArrayPool<byte>.Shared.Rent(size);

                for (var i = 0; i < size; i++)
                {
                    buffer[i] = (byte) (_buffer[offset + i] ^ Constants.ServerBits[_seed]);
                    _seed = (ushort) ((_seed + 1) % Constants.ServerBits.Length);
                }

                Server.Queue.Enqueue(NetworkEvent.Create(
                    connection: this, 
                    code: NetworkEventCode.EVENT_MESSAGE,
                    message: Message.Create(this, BitConverter.ToUInt16(buffer), buffer, size)));

                offset += size;
                bytes -= size;
            }

            BeginReceive();
        }
        catch (SocketException exception) when (exception.SocketErrorCode == SocketError.ConnectionReset)
        {
            Disconnect();
        }
    }
}

/// <summary>
/// Something that occurs within the lifetime of a <see cref="Networking.Connection"/>.
/// </summary>
internal class NetworkEvent : IDisposable
{
    private static readonly ConcurrentBag<NetworkEvent> _pool = [];

    private Connection _connection;
    private NetworkEventCode _code;
    private Message? _message;

    private NetworkEvent(Connection connection, NetworkEventCode code, Message? message = null)
    {
        _connection = connection;
        _code = code;
        _message = message;
    }

    /// <summary>
    /// A <see cref="Networking.Connection"/> identification number.
    /// </summary>
    public Connection Connection => _connection;

    /// <summary>
    /// A classifying <see cref="NetworkEventCode"/>.
    /// </summary>
    public NetworkEventCode Code => _code;

    /// <summary>
    /// The <see cref="Networking.Message"/> associated with the <see cref="NetworkEvent"/>.
    /// </summary>
    public Message? Message => _message;

    /// <summary>
    /// Create a new <see cref="NetworkEvent"/>.
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="code">A classifying <see cref="NetworkEventCode"/>.</param>
    /// <param name="message">The <see cref="Networking.Message"/> associated with the <see cref="NetworkEvent"/>.</param>
    /// <returns>A <see cref="NetworkEvent"/>.</returns>
    public static NetworkEvent Create(Connection connection, NetworkEventCode code, Message? message = null)
    {
        if (!_pool.TryTake(out var @event))
        {
            @event = new(connection, code, message);
        }
        else
        {
            @event._connection = connection;
            @event._code = code;
            @event._message = message;
        }

        return @event;
    }

    /// <summary>
    /// Dispose of the <see cref="NetworkEvent"/>.
    /// </summary>
    public void Dispose()
    {
        _pool.Add(this);
    }
}

/// <summary>
/// Describes a <see cref="NetworkEvent"/>.
/// </summary>
internal enum NetworkEventCode
{
    /// <summary>
    /// The <see cref="Connection"/> was opened.
    /// </summary>
    EVENT_CONNECT,

    /// <summary>
    /// The <see cref="Connection"/> sent a message to the <see cref="Server"/>.
    /// </summary>
    EVENT_MESSAGE,

    /// <summary>
    /// The <see cref="Connection"/> was closed.
    /// </summary>
    EVENT_DISCONNECT
}

/// <summary>
/// A <see cref="Message"/> sent to or from the <see cref="Server"/>.
/// </summary>
internal class Message : IDisposable
{
    private static readonly ConcurrentBag<Message> _pool = [];

    private Connection _connection;
    private ushort _command;
    private byte[] _data;
    private int _size;

    private Message(Connection connection, ushort command, byte[] data, int size)
    {
        _connection = connection;
        _command = command;
        _data = data;
        _size = size;
    }

    /// <summary>
    /// The <see cref="Networking.Connection"/> associated with the <see cref="Message"/>.
    /// </summary>
    public Connection Connection => _connection;

    /// <summary>
    /// The message's command.
    /// </summary>
    public ushort Command => _command;

    /// <summary>
    /// The message's data.
    /// </summary>
    public byte[] Data => _data;

    /// <summary>
    /// The size of the <see cref="Message"/>.
    /// </summary>
    public int Size => _size;

    /// <summary>
    /// Create a new <see cref="Message"/>.
    /// </summary>
    /// <param name="connection">The <see cref="Networking.Connection"/> associated with the message.</param>
    /// <param name="command">The message's command.</param>
    /// <param name="data">The message's data.</param>
    /// <param name="size">The size of the <see cref="Message"/>.</param>
    /// <returns>A <see cref="Message"/>.</returns>
    public static Message Create(Connection connection, ushort command, byte[] data, int size)
    {
        if (!_pool.TryTake(out var message))
        {
            message = new(connection, command, data, size);
        }
        else
        {
            message._connection = connection;
            message._command = command;
            message._data = data;
            message._size = size;
        }

        return message;
    }

    /// <summary>
    /// Dispose of the <see cref="Message"/>.
    /// </summary>
    public void Dispose()
    {
        _pool.Add(this);
    }
}