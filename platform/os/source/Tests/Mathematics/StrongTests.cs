// Copyright © Spatial. All rights reserved.

using Spatial.Mathematics;

namespace Spatial.Hardware;

/// <summary>
/// Unit tests for <see cref="Strong"/>.
/// </summary>
public class StrongTests
{
    /// <summary>
    /// Test <see cref="Strong.Float"/>.
    /// </summary>
    [Fact]
    public void TestFloat()
    {
        Strong.Float();
    }

    /// <summary>
    /// Test <see cref="Strong.Double"/>.
    /// </summary>
    [Fact]
    public void TestDouble()
    {
        Strong.Double();
    }

    /// <summary>
    /// Test <see cref="Strong.Byte"/>.
    /// </summary>
    [Fact]
    public void TestByte()
    {
        Strong.Byte();
    }

    /// <summary>
    /// Test <see cref="Strong.UInt16"/>.
    /// </summary>
    [Fact]
    public void TestUInt16()
    {
        Strong.UInt16();
    }

    /// <summary>
    /// Test <see cref="Strong.Int32"/>.
    /// </summary>
    [Fact]
    public void TestInt32()
    {
        Strong.Int32();
    }

    /// <summary>
    /// Test <see cref="Strong.Int64"/>.
    /// </summary>
    [Fact]
    public void TestInt64()
    {
        Strong.Int64();
    }

    /// <summary>
    /// Test <see cref="Strong.Bytes"/>.
    /// </summary>
    [Fact]
    public void TestBytes()
    {
        Assert.Equal(4, Strong.Bytes(4).Length);
    }
}
