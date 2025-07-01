// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Simulation.Components;

namespace Spatial.Simulation;

/// <summary>
/// Unit tests for <see cref="Archetype"/>.
/// </summary>
public class ArchetypeTests
{
    /// <summary>
    /// Test the <see cref="Archetype"/> constructor.
    /// </summary>
    [Fact]
    public void TestConstructor()
    {
        var space = new Space();
        var signature = Signature.Empty;
        var archetype = new Archetype(space, 0, signature);

        Assert.Equal(space, archetype.Space);
        Assert.Equal(signature, archetype.Signature);
        Assert.Single(archetype.Chunks);
    }

    /// <summary
    /// Test <see cref="Archetype.Reserve(uint)"/>.
    /// </summary>
    [Fact]
    public void TestReserve()
    {
        var signature = Signature.Combine<Position, Velocity>();
        var archetype = new Archetype(new(), 0, signature);

        archetype.Reserve(10000);

        Assert.Equal(6, archetype.Chunks.Length);
    }

    /// <summary>
    /// Test <see cref="Archetype.Add(Entity)"/>.
    /// </summary>
    [Fact]
    public void TestAdd()
    {
        var signature = Signature.Combine<Position, Velocity>();
        var archetype = new Archetype(new(), 0, signature);
        var entity = new Entity();

        archetype.Add(ref entity);

        Assert.Equal((uint) 1, archetype.Chunks[0].Count);
        Assert.Equal((uint) 0, entity.Chunk);
        Assert.Equal((uint) 0, entity.Index);
    }

    /// <summary>
    /// Test <see cref="Archetype.Remove(ushort, ushort)"/>.
    /// </summary>
    [Fact]
    public void TestRemove()
    {
        var signature = Signature.Empty;
        var archetype = new Archetype(new(), 0, signature);
        var entity = archetype.Space.Create();

        archetype.Add(ref entity);
        archetype.Remove(entity);

        Assert.Equal((uint) 0, archetype.Chunks[0].Count);
    }

    /// <summary>
    /// Test <see cref="Archetype.Dispose"/>.
    /// </summary>
    [Fact]
    public void TestDispose()
    {
        new Archetype(new(), 0, Signature.Empty).Dispose();
    }
}