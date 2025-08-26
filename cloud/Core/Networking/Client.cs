// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Networking.Contracts.Miscellaneous;
using System.Buffers;
using System.Net;
using System.Net.Sockets;

namespace Spatial.Networking;

/// <summary>
/// A device that communicates with the <see cref="Network"/>.
/// </summary>
public class Client : IDisposable
{
    private readonly Dictionary<ushort, List<(Action<Client, ProtocolBuffer> Handler, Type Prototype)>> _handlers;
    private readonly byte[] _buffer;

    private Socket _socket;
    private byte _connected;
    private ushort _seed;

    /// <summary>
    /// Create a new <see cref="Client"/>.
    /// </summary>
    public Client()
    {
        _handlers = [];
        _buffer = ArrayPool<byte>.Shared.Rent(Constants.ConnectionSize);

        Handle<PROTO_NC_MISC_SEED_CMD>(
            command: NETCOMMAND.NC_MISC_SEED_CMD, 
            handler: (_, data) => Interlocked.Exchange(ref _seed, data.Seed));
    }

    /// <summary>
    /// Whether or not the <see cref="Client"/> is connected.
    /// </summary>
    public bool Connected => Interlocked.CompareExchange(ref _connected, 1, 1) == 1;

    /// <summary>
    /// The client's remote endpoint.
    /// </summary>
    public IPEndPoint Endpoint => _socket.RemoteEndPoint as IPEndPoint ?? throw new InvalidOperationException("The client is not connected.");

    /// <summary>
    /// Handle a <see cref="NETCOMMAND"/>.
    /// </summary>
    /// <typeparam name="T">The type of data to bind the <see cref="NETCOMMAND"/> to.</typeparam>
    /// <param name="command">A <see cref="NETCOMMAND"/>.</param>
    /// <param name="handler">A handler.</param>
    /// <returns>The <see cref="Client"/>.</returns>
    public Client Handle<T>(ushort command, Action<Client, T> handler) where T : ProtocolBuffer
    {
        if (!_handlers.TryGetValue(command, out var handlers))
        {
            handlers = _handlers[command] = [];
        }
        
        handlers.Add(((client, data) => handler(client, (T) data), typeof(T)));

        return this;
    }

    /// <summary>
    /// Connect the <see cref="Client"/>.
    /// </summary>
    /// <param name="endpoint">A <see cref="Network"/> endpoint.</param>
    public void Connect(string endpoint) => Connect(IPEndPoint.Parse(endpoint));

    /// <summary>
    /// Connect the <see cref="Client"/>.
    /// </summary>
    /// <param name="port">A <see cref="Network"/> port.</param>
    public void Connect(int port) => Connect(new IPEndPoint(IPAddress.Loopback, port));

    /// <summary>
    /// Connect the <see cref="Client"/>.
    /// </summary>
    /// <param name="endpoint">A <see cref="Network"/> endpoint.</param>
    public void Connect(IPEndPoint endpoint)
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _socket.Connect(endpoint);
        _connected = 1;

        Receive();
    }

    /// <summary>
    /// Issue a <see cref="NETCOMMAND"/> to the <see cref="Client"/>.
    /// </summary>
    /// <param name="command">A <see cref="NETCOMMAND"/>.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/>.</param>
    public void Command(ushort command, ProtocolBuffer data, bool dispose = true)
    {
        if (!Connected)
        {
            return;
        }

        data.Serialize(true);

        var array = data.ToArray();
        var size = array.Length + sizeof(ushort);

        if (dispose)
        {
            data.Dispose();
        }

        byte[] buffer;

        if (size <= byte.MaxValue)
        {
            buffer = new byte[size + 1];
            buffer[0] = (byte) size;
            
            Buffer.BlockCopy(BitConverter.GetBytes(command), 0, buffer, 1, sizeof(ushort));
            Buffer.BlockCopy(array, 0, buffer, 3, array.Length);

            Encode(new ArraySegment<byte>(buffer, 1, buffer.Length - 1));
        }
        else
        {
            buffer = new byte[size + 3];
            buffer[0] = 0;
            
            Buffer.BlockCopy(BitConverter.GetBytes((ushort) size), 0, buffer, 1, sizeof(ushort));
            Buffer.BlockCopy(BitConverter.GetBytes(command), 0, buffer, 3, sizeof(ushort));
            Buffer.BlockCopy(array, 0, buffer, 5, array.Length);

            Encode(new ArraySegment<byte>(buffer, 3, buffer.Length - 3));
        }

        _socket.Send(buffer);
    }

    /// <summary>
    /// Disconnect the client.
    /// </summary>
    public void Disconnect()
    {
        if (Interlocked.CompareExchange(ref _connected, 0, 1) == 1)
        {
            _socket.Close();
        }
    }

    /// <summary>
    /// Dispose of the <see cref="Client"/>.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void Dispose()
    {
        Disconnect();

        ArrayPool<byte>.Shared.Return(_buffer);

        _socket.Dispose();
    }

    private void Encode(ArraySegment<byte> data)
    {
        for (var i = 0; i < data.Count; i++)
        {
            ushort seed, update;

            do
            {
                seed = Volatile.Read(ref _seed);
                update = (ushort) ((seed + 1) % Constants.ServerBits.Length);
            }
            while (Interlocked.CompareExchange(ref _seed, update, seed) != seed);

            data.Array![data.Offset + i] ^= Constants.ServerBits[seed];
        }
    }

    private void Receive()
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
                
                Process(new ArraySegment<byte>(_buffer, offset, size));

                offset += size;
                bytes -= size;
            }

            Receive();
        }
        catch (SocketException exception) when (exception.SocketErrorCode == SocketError.ConnectionReset)
        {
            Disconnect();
        }
    }

    private void Process(ArraySegment<byte> buffer)
    {
        if (!_handlers.TryGetValue(BitConverter.ToUInt16(buffer), out var handlers))
        {
            return;
        }

        foreach (var (handler, prototype) in handlers)
        {
            handler(this, ProtocolBuffer.FromSpan(prototype, buffer.AsSpan(2, buffer.Count - 2)));
        }
    }
}