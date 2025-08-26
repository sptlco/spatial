// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Simulation.Components;

namespace Spatial.Simulation;

/// <summary>
/// Unit tests for <see cref="Query"/>.
/// </summary>
public class QueryTests
{
    /// <summary>
    /// Test <see cref="Query.WithAll(in Signature)"/>.
    /// </summary>
    [Fact]
    public void TestWithAll()
    {
        var query = new Query().WithAll(Signature.Combine<Position, Velocity>());
        var signature = Signature.Combine<Position, Velocity>();

        Assert.True(query.Matches(signature));
    }   

    /// <summary>
    /// Test <see cref="Query.WithAny(in Signature)"/>.
    /// </summary>
    [Fact]
    public void TestWithAny()
    {
        var query = new Query().WithAny(Signature.Combine<Position, Velocity>());
        var signature = Signature.Of<Velocity>();

        Assert.True(query.Matches(signature));
    }

    /// <summary>
    /// Test <see cref="Query.WithNone(in Signature)"/>.
    /// </summary>
    [Fact]
    public void TestWithNone()
    {
        var query = new Query().WithNone(Signature.Of<Position>());
        var signature1 = Signature.Combine<Position, Velocity>();
        var signature2 = Signature.Of<Velocity>();

        Assert.False(query.Matches(signature1));
        Assert.True(query.Matches(signature2));
    }

    /// <summary>
    /// Test <see cref="Query.Matches(in Signature)"/>.
    /// </summary>
    [Fact]
    public void TestMatches()
    {
        var query = new Query().WithNone(Signature.Of<Position>());
        var signature1 = Signature.Combine<Position, Velocity>();
        var signature2 = Signature.Of<Velocity>();

        Assert.False(query.Matches(signature1));
        Assert.True(query.Matches(signature2));
    }
}