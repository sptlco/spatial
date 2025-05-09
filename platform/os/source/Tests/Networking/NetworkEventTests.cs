// Copyright © Spatial. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// Unit tests for <see cref="NetworkEvent"/>.
/// </summary>
public class SessionEventTests
{
    /// <summary>
    /// Test the <see cref="NetworkEvent"/> constructor.
    /// </summary>
    [Fact]
    public void TestSessionEvent()
    {
        var connection = new Connection();
        var e = NetworkEvent.Create(connection, NetworkEventCode.EVENT_CONNECT);

        Assert.Equal(connection, e.Connection);
        Assert.Equal(NetworkEventCode.EVENT_CONNECT, e.Code);
        Assert.Null(e.Message);
    }
}
