// Copyright © Spatial Corporation. All rights reserved.

using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;

namespace Spatial.Networking;

/// <summary>
/// A bidirectional proxy for web sockets.
/// </summary>
public class Bridge
{
    private readonly Connection _connection;
    private readonly (WebSocket Web, Socket Native) _socket;

    private Bridge(WebSocket socket)
    {
        _socket = (socket, CreateSocket());
        _connection = Application.Current.Network.Connect(_socket.Native, this);
    }

    /// <summary>
    /// Start a new <see cref="Bridge"/>.
    /// </summary>
    /// <param name="socket">The bridge's <see cref="WebSocket"/>.</param>
    /// <returns>The <see cref="Bridge"/> that was started.</returns>
    public static Bridge StartNew(WebSocket socket)
    {
        return new Bridge(socket);
    }

    /// <summary>
    /// The <see cref="Networking.Connection"/> under the <see cref="Bridge"/>.
    /// </summary>
    public Connection Connection => _connection;

    /// <summary>
    /// The bridge's sockets.
    /// </summary>
    public (WebSocket Web, Socket Native) Socket => _socket;

    /// <summary>
    /// Connect the <see cref="Bridge"/>.
    /// </summary>
    public void Connect()
    {
        _ = ForwardAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Forward data from the <see cref="WebSocket"/> to the <see cref="Networking.Connection"/>.
    /// </summary>
    public async Task ForwardAsync()
    {
        try
        {
            while (_socket.Web.State == WebSocketState.Open)
            {
                var result = await _socket.Web.ReceiveAsync(_connection.Buffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _connection.Disconnect();

                    break;
                }

                _connection.Process(result.Count);
            }
        }
        catch { }
    }

    private Socket CreateSocket()
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var endpoint = (Application.Current.Network.Endpoints.First().LocalEndPoint as IPEndPoint)!;

        socket.Connect(IPAddress.Loopback, endpoint.Port);

        return socket;
    }
}