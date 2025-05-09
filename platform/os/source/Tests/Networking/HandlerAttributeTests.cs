// Copyright © Spatial. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// Unit tests for <see cref="HandlerAttribute"/>.
/// </summary>
public class HandlerAttributeTests
{
    /// <summary>
    /// Test the <see cref="HandlerAttribute"/> constructor.
    /// </summary>
    [Fact]
    public void TestHandlerAttribute()
    {
        var attribute = new HandlerAttribute(10);

        Assert.Equal(10, attribute.Command);
    }
}
