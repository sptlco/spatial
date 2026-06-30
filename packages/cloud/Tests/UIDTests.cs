// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

/// <summary>
/// Tests for a <see cref="UID"/>.
/// </summary>
public class UIDTests
{
    /// <summary>
    /// Convert a <see cref="UID"/> to a string.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestToString()
    {
        Assert.NotEmpty(new UID().ToString());
    }
}
