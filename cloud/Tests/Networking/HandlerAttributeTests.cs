// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// Unit tests for <see cref="OperationAttribute"/>.
/// </summary>
public class HandlerAttributeTests
{
    /// <summary>
    /// Test the <see cref="OperationAttribute"/> constructor.
    /// </summary>
    [Fact]
    public void TestHandlerAttribute()
    {
        var attribute = new OperationAttribute(10);

        Assert.Equal(10, attribute.Code);
    }
}
