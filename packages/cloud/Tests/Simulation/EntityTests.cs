// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Simulation;

/// <summary>
/// Unit tests for <see cref="Entity"/>.
/// </summary>
public class EntityTests
{
    /// <summary>
    /// Test <see cref="Entity.Handle"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestHandle()
    {
        Assert.Equal((ulong) 0, new Entity().Handle);
    }

    /// <summary>
    /// Test <see cref="Entity.Id"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestId()
    {
        Assert.Equal((uint) 1, new Entity(1).Id);
    }

    /// <summary>
    /// Test <see cref="Entity.Archetype"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestArchetype()
    {
        var entity = new Entity {
            Archetype = 1
        };

        Assert.Equal((ushort) 1, entity.Archetype);
    }

    /// <summary>
    /// Test <see cref="Entity.Chunk"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestChunk()
    {
        var entity = new Entity {
            Chunk = 1
        };

        Assert.Equal((ushort) 1, entity.Chunk);
    }

    /// <summary>
    /// Test <see cref="Entity.Index"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestIndex()
    {
        var entity = new Entity {
            Index = 1
        };

        Assert.Equal((uint) 1, entity.Index);
    }
}