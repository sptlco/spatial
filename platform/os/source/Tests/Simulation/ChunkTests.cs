// Copyright © Spatial. All rights reserved.

using Spatial.Simulation.Components;

namespace Spatial.Simulation;

/// <summary>
/// Unit tests for <see cref="Chunk"/>.
/// </summary>
public class ChunkTests
{
    /// <summary>
    /// Test the <see cref="Chunk"/> constructor.
    /// </summary>
    [Fact]
    public void TestConstructor()
    {
        var archetype = new Archetype(new(), 0, Signature.Empty);
        var chunk = new Chunk(archetype, 0);

        Assert.Equal(archetype, chunk.Archetype);
        Assert.NotNull(chunk.Components);
    }

    /// <summary>
    /// Test <see cref="Chunk.Emplace(ref Entity)"/>.
    /// </summary>
    [Fact]
    public void TestEmplace()
    {
        var archetype = new Archetype(new(), 0, Signature.Empty);
        var chunk = new Chunk(archetype, 0);
        var entity = new Entity();

        chunk.Emplace(entity);

        Assert.Equal((uint) 1, chunk.Count);
        Assert.Equal((uint) 0, entity.Chunk);
        Assert.Equal((uint) 0, entity.Index);
    }

    /// <summary>
    /// Test <see cref="Chunk.Set{T}(int, int, in T)"/>.
    /// </summary>
    [Fact]
    public void TestSet()
    {
        var signature = Signature.Combine<Position, Velocity>();
        var archetype = new Archetype(new(), 0, signature);
        var chunk = new Chunk(archetype, 0);

        chunk.Set(3, new Position(10, 3, 4));
    }

    /// <summary>
    /// Test <see cref="Chunk.Ref{T}(int, int)"/>.
    /// </summary>
    [Fact]
    public void TestRef()
    {
        var signature = Signature.Combine<Position, Velocity>();
        var archetype = new Archetype(new(), 0, signature);
        var chunk = new Chunk(archetype, 0);

        chunk.Set(3, new Position(10, 3, 4));
        
        ref var position = ref chunk.Ref<Position>(3);
        
        Assert.Equal(10, position.X);
        Assert.Equal(3, position.Y);
        Assert.Equal(4, position.Z);
    }

    /// <summary>
    /// Test <see cref="Chunk.Dispose"/>.
    /// </summary>
    [Fact]
    public void TestDispose()
    {
        new Chunk(new(new(), 0, Signature.Empty), 0).Dispose();
    }
}
