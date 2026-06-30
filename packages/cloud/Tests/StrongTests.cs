// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

/// <summary>
/// Tests for <see cref="Strong"/>.
/// </summary>
public class StrongTests
{
    /// <summary>
    /// Generate a random boolean.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestBoolean()
    {
        Assert.True(Strong.Boolean(1.0));
        Assert.False(Strong.Boolean(0.0));    
    }

    /// <summary>
    /// Generate a random float.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestFloat()
    {
        Assert.True(Strong.Float(0, 48) < 48);
    }

    /// <summary>
    /// Generate a random double.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestDouble()
    {
        Assert.True(Strong.Double(0, 18) < 18);
    }

    /// <summary>
    /// Generate a random byte.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestByte()
    {
        Assert.True(Strong.Byte(0, 128) < 128);
    }

    /// <summary>
    /// Generate a random unsigned 16-bit integer.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestUInt16()
    {
        Assert.True(Strong.UInt16(0, 12000) < 12000);
    }

    /// <summary>
    /// Generate a random 32-bit integer.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestInt32()
    {
        Assert.True(Strong.Int32(0, 500) < 500);
    }

    /// <summary>
    /// Generate a random 64-bit integer.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestInt64()
    {
        Assert.True(Strong.Int64(0, 68) < 68);
    }

    /// <summary>
    /// Generate a random byte array.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestBytes()
    {
        Assert.Equal(5, Strong.Bytes(5).Length);
    }
}