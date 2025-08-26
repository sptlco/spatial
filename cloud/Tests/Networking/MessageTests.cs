// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// Unit tests for <see cref="Message"/>.
/// </summary>
public class MessageTests
{
    /// <summary>
    /// Test the <see cref="Message"/> constructor.
    /// </summary>
    [Fact]
    public void TestMessage()
    {
        var connection = new Connection();
        var message = Message.Create(connection, 2, [], 10);

        Assert.Equal(connection, message.Connection);
        Assert.Equal(2, message.Command);
        Assert.Empty(message.Data);
        Assert.Equal(10, message.Size);
    }
}
