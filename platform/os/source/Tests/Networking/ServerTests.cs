// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// Unit tests for <see cref="Server"/>.
/// </summary>
public class ServerTests
{   
    /// <summary>
    /// Test <see cref="Server.Open"/>.
    /// </summary>
    [Fact]
    public void TestOpen()
    {
        Server.Open(5000);

        Assert.Equal("127.0.0.1", Server.Endpoint.Address.ToString());
        Assert.Equal(5000, Server.Endpoint.Port);
    }

    /// <summary>
    /// Test <see cref="Server.Receive"/>.
    /// </summary>
    [Fact]
    public void TestReceive()
    {
        Server.Open();

        Server.Queue.Enqueue(NetworkEvent.Create(new Connection(), NetworkEventCode.EVENT_CONNECT));
        Server.Receive();

        Assert.True(Server.Queue.Empty);
    }

    /// <summary>
    /// Test <see cref="Server.Send"/>.
    /// </summary>
    [Fact]
    public void TestSend()
    {
        Server.Open();

        Server.Send();
    }
}