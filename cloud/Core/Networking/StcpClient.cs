// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Helpers;
using Spatial.Networking.Contracts;
using Spatial.Networking.Contracts.Miscellaneous;
using System.Buffers;
using System.Net;
using System.Net.Sockets;

namespace Spatial.Networking;

/// <summary>
/// A device that communicates with the <see cref="Network"/>.
/// </summary>
public class StcpClient : IDisposable
{
    private readonly Dictionary<ushort, List<(Action<StcpClient, ProtocolBuffer> Handler, Type Prototype)>> _handlers;
    private readonly byte[] _buffer;

    private Socket _socket;
    private byte _connected;
    private byte _secure;
    private ushort _seed;
    private int _key1;
    private int _key2;
    private byte[] _keystream;

    /// <summary>
    /// Create a new <see cref="StcpClient"/>.
    /// </summary>
    internal StcpClient()
    {
        _handlers = [];
        _buffer = ArrayPool<byte>.Shared.Rent(Constants.ConnectionSize);

        Handle<PROTO_NC_MISC_SEED_CMD>(
            command: (ushort) NETCOMMAND.NC_MISC_SEED_CMD, 
            handler: (_, data) => {
                Interlocked.Exchange(ref _secure, 1);
                Interlocked.Exchange(ref _key1, 0);
                Interlocked.Exchange(ref _key2, 0);
                Interlocked.Exchange(ref _seed, data.Seed);
                Interlocked.Exchange(ref _keystream, Cipher.GenerateKeystream(data.Seed));
            });
    }

    /// <summary>
    /// Whether or not the <see cref="StcpClient"/> is connected.
    /// </summary>
    public bool Connected => Interlocked.CompareExchange(ref _connected, 1, 1) == 1;

    /// <summary>
    /// The client's current seed.
    /// </summary>
    public ushort Seed => Interlocked.CompareExchange(ref _seed, 1, 1);

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
    /// <returns>The <see cref="StcpClient"/>.</returns>
    public StcpClient Handle<T>(ushort command, Action<StcpClient, T> handler) where T : ProtocolBuffer
    {
        if (!_handlers.TryGetValue(command, out var handlers))
        {
            handlers = _handlers[command] = [];
        }
        
        handlers.Add(((sender, data) => handler(sender, (T) data), typeof(T)));

        return this;
    }

    /// <summary>
    /// Connect the <see cref="StcpClient"/>.
    /// </summary>
    /// <param name="endpoint">A <see cref="Network"/> endpoint.</param>
    internal StcpClient Connect(string endpoint) => Connect(IPEndPoint.Parse(endpoint));

    /// <summary>
    /// Connect the <see cref="StcpClient"/>.
    /// </summary>
    /// <param name="port">A <see cref="Network"/> port.</param>
    internal StcpClient Connect(int port) => Connect(new IPEndPoint(IPAddress.Loopback, port));

    /// <summary>
    /// Connect the <see cref="StcpClient"/>.
    /// </summary>
    /// <param name="endpoint">A <see cref="Network"/> endpoint.</param>
    internal StcpClient Connect(IPEndPoint endpoint)
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _socket.Connect(endpoint);
        _connected = 1;

        Receive();

        return this;
    }

    /// <summary>
    /// Send a message to the <see cref="Network"/>.
    /// </summary>
    /// <param name="command">A <see cref="NETCOMMAND"/>.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/>.</param>
    public void Send(ushort command, ProtocolBuffer data, bool dispose = true)
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

        Cipher.Transform(array, 0, array.Length, _keystream, ref _key1);

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
    /// Dispose of the <see cref="StcpClient"/>.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void Dispose()
    {
        Disconnect();

        ArrayPool<byte>.Shared.Return(_buffer);

        _socket.Dispose();
    }

    private void Encode(ArraySegment<byte> data) => Cipher.Transform(data.Array!, data.Offset, data.Count, _keystream, ref _key1);

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
            if (Interlocked.CompareExchange(ref _secure, 1, 1) == 1)
            {
                Cipher.Transform(buffer.Array!, 2, buffer.Count - 2, _keystream, ref _key2);
            }

            handler(this, ProtocolBuffer.FromSpan(prototype, buffer.AsSpan(2, buffer.Count - 2)));
        }
    }
}