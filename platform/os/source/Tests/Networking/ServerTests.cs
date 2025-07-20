// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// Unit tests for <see cref="Network"/>.
/// </summary>
public class ServerTests
{   
    /// <summary>
    /// Test <see cref="Network.Open"/>.
    /// </summary>
    [Fact]
    public void TestOpen()
    {
        Network.Open(5000);

        Assert.Equal("127.0.0.1", Network.Endpoint.Address.ToString());
        Assert.Equal(5000, Network.Endpoint.Port);
    }

    /// <summary>
    /// Test <see cref="Network.Receive"/>.
    /// </summary>
    [Fact]
    public void TestReceive()
    {
        Network.Open();

        Network.Queue.Enqueue(NetworkEvent.Create(new Connection(), NetworkEventCode.EVENT_CONNECT));
        Network.Receive();

        Assert.True(Network.Queue.Empty);
    }

    /// <summary>
    /// Test <see cref="Network.Send"/>.
    /// </summary>
    [Fact]
    public void TestSend()
    {
        Network.Open();

        Network.Send();
    }
}