// Copyright © Spatial Corporation. All rights reserved.

using System.Text;

namespace Spatial.Extensions;

/// <summary>
/// Tests for <see cref="ByteArrayExtensions"/>.
/// </summary>
public class ByteArrayExtensionTests
{
    /// <summary>
    /// Convert a byte array to an MD5 string.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestToMD5()
    {
        Assert.Equal("6cd3556deb0da54bca060b4c39479839", Encoding.ASCII.GetBytes("Hello, world!").ToMD5());
    }
}
