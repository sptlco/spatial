// Copyright © Spatial. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// Unit tests for <see cref="NetworkEventCode"/>.
/// </summary>
public class SessionEventCodeTests
{
    /// <summary>
    /// Test <see cref="NetworkEventCode"/> enumeration.
    /// </summary>
    [Fact]
    public void TestSessionEventCode()
    {
        Assert.Equal(0, (int) NetworkEventCode.EVENT_CONNECT);
        Assert.Equal(1, (int) NetworkEventCode.EVENT_MESSAGE);
        Assert.Equal(2, (int) NetworkEventCode.EVENT_DISCONNECT);
    }
}
