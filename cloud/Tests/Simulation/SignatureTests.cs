// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Simulation.Components;

namespace Spatial.Simulation;

/// <summary>
/// Unit tests for <see cref="Signature"/>.
/// </summary>
public class SignatureTests
{
    /// <summary>
    /// Test the <see cref="Signature"/> constructor.
    /// </summary>
    [Fact]
    public void TestConstructor()
    {
        var signature = new Signature();

        Assert.Equal(0, signature.Count);
        Assert.Empty(signature.Components);
    }

    /// <summary>
    /// Test <see cref="Signature.Empty"/>.
    /// </summary>
    [Fact]
    public void TestEmpty()
    {
        Assert.Equal(0, Signature.Empty.Count);
        Assert.Empty(Signature.Empty.Components);
    }

    /// <summary>
    /// Test <see cref="Signature.Count"/>.
    /// </summary>
    [Fact]
    public void TestCount()
    {
        var signature = new Signature();

        Assert.Equal(0, signature.Count);

        signature = new Signature(1);

        Assert.Equal(1, signature.Count);
    }

    /// <summary>
    /// Test <see cref="Signature.Components"/>.
    /// </summary>
    [Fact]
    public void TestComponents()
    {
        var signature = new Signature(0);

        Assert.Empty(signature.Components);

        signature = new Signature(1);

        Assert.Single(signature.Components);
    }

    /// <summary>
    /// Test <see cref="Signature.operator |"/>.
    /// </summary>
    [Fact]
    public void TestOr()
    {
        var left = new Signature(1);
        var right = new Signature(2);

        var result = left | right;

        Assert.Equal(2, result.Count);
        Assert.Equal([typeof(Position), typeof(Velocity)], result.Components);
    }

    /// <summary>
    /// Test <see cref="Signature.operator &"/>.
    /// </summary>
    [Fact]
    public void TestAnd()
    {
        var left = new Signature(3);
        var right = new Signature(2);

        var result = left & right;

        Assert.Equal(1, result.Count);
        Assert.Equal([typeof(Velocity)], result.Components);
    }

    /// <summary>
    /// Test <see cref="Signature.operator ~"/>.
    /// </summary>
    [Fact]
    public void TestNot()
    {
        Assert.Equal(127, (~new Signature(1)).Count);
    }

    /// <summary>
    /// Test <see cref="Signature.operator =="/>.
    /// </summary>
    [Fact]
    public void TestEqual()
    {
        var left = new Signature(1);
        var right = new Signature(1);

        Assert.True(left == right);
    }

    /// <summary>
    /// Test <see cref="Signature.operator !="/>.
    /// </summary>
    [Fact]
    public void TestNotEqual()
    {
        var left = new Signature(1);
        var right = new Signature(2);

        Assert.True(left != right);
    }

    /// <summary>
    /// Test <see cref="Signature.All(in Signature)"/>.
    /// </summary>
    [Fact]
    public void TestAll()
    {
        var signature1 = Signature.Combine<Position>();
        var signature2 = Signature.Combine<Position, Velocity>();

        Assert.True(signature2.All(signature1));
    }

    /// <summary>
    /// Test <see cref="Signature.Any(in Signature)"/>.
    /// </summary>
    [Fact]
    public void TestAny()
    {
        var signature1 = Signature.Combine<Position>();
        var signature2 = Signature.Combine<Position, Velocity>();

        Assert.True(signature1.Any(signature2));
    }

    /// <summary>
    /// Test <see cref="Signature.None(in Signature)"/>.
    /// </summary>
    [Fact]
    public void TestNone()
    {
        var signature1 = Signature.Combine<Position>();
        var signature2 = Signature.Empty;

        Assert.True(signature2.None(signature1));
    }

    /// <summary>
    /// Test <see cref="Signature.Equals(object)"/>.
    /// </summary>
    [Fact]
    public void TestEquals()
    {
        var left = new Signature(1);
        var right = new Signature(1);

        Assert.True(left.Equals(right));
    }

    /// <summary>
    /// Test <see cref="Signature.GetHashCode"/>.
    /// </summary>
    [Fact]
    public void TestGetHashCode()
    {
        var signature = new Signature(1);

        Assert.Equal(signature.GetHashCode(), signature.GetHashCode());
    }

    /// <summary>
    /// Test <see cref="Signature.Of{T}"/>.
    /// </summary>
    [Fact]
    public void TestOf()
    {
        var signature = Signature.Of<Position>();

        Assert.Equal(1, signature.Count);
        Assert.Equal([typeof(Position)], signature.Components);
    }

    /// <summary>
    /// Test <see cref="Signature.Combine(Signature[])"/>.
    /// </summary>
    [Fact]
    public void TestCombine()
    {
        var result = Signature.Combine<Position, Velocity>();

        Assert.Equal(2, result.Count);
        Assert.Equal([typeof(Position), typeof(Velocity)], result.Components);
    }

    /// <summary>
    /// Test <see cref="Signature.Includes{T}"/>.
    /// </summary>
    [Fact]
    public void TestIncludes()
    {
        var signature = Signature.Combine<Position, Velocity>();

        Assert.True(signature.Includes<Position>());
        Assert.True(signature.Includes<Velocity>());
    }
}
