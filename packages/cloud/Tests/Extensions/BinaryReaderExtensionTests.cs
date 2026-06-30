// Copyright © Spatial Corporation. All rights reserved.

using Nethereum.Model;
using System.Text;

namespace Spatial.Extensions;

/// <summary>
/// Tests for <see cref="BinaryReaderExtensions"/>.
/// </summary>
public class BinaryReaderExtensionTests
{
    /// <summary>
    /// Read a string.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestReadString()
    {
        var bytes = Encoding.ASCII.GetBytes("Hello, world!");

        using var stream = new MemoryStream(bytes);
        using var reader = new BinaryReader(stream);

        Assert.Equal("Hello, world!", reader.ReadString(13));
    }
}
