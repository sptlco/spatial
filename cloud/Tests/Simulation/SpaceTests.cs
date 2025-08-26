// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Simulation.Components;
using Spatial.Simulation.Systems;

namespace Spatial.Simulation;

/// <summary>
/// Unit tests for <see cref="Space"/>.
/// </summary>
public class SpaceTests
{
    /// <summary>
    /// Test the <see cref="Space"/> constructor.
    /// </summary>
    [Fact]
    public void TestConstructor()
    {
        var space = new Space();

        Assert.Single(space.Entities);
        Assert.Single(space.Archetypes);
    }

    /// <summary>
    /// Test <see cref="Space.Empty"/>.
    /// </summary>
    [Fact]
    public void TestEmpty()
    {
        var space = Space.Empty();
        var query = new Query().Parallel().WithAll<Position>();

        Assert.Single(space.Archetypes);
        Assert.Single(space.Entities);
    }

    /// <summary>
    /// Test <see cref="Space.Update"/>.
    /// </summary>
    [Fact]
    public void TestUpdate()
    {
        var counter = new Counter();
        var space = Space.Empty().Use(counter);

        space.Update(33);

        Assert.Equal(3, counter.Count);
    }

    /// <summary>
    /// Test <see cref="Space.UseUpdate{T}()"/>.
    /// </summary>
    [Fact]
    public void TestUse()
    {
        var counter = new Counter();
        var space = Space.Empty().Use(_ => counter);

        space.Update(33);

        Assert.Equal(3, counter.Count);
    }

    /// <summary
    /// Test <see cref="Space.Reserve(in Signature, uint)"/>.
    /// </summary>
    [Fact]
    public void TestReserve()
    {
        var space = new Space();
        var signature = Signature.Combine<Position, Velocity>();

        space.Reserve(signature, 10000);

        Assert.Equal(6, space.Archetypes[1]!.Chunks.Length);
    }

    /// <summary>
    /// Test <see cref="Space.Create"/>.
    /// </summary>
    [Fact]
    public void TestCreate()
    {
        var space = new Space();
        var entity = space.Create();

        Assert.Equal(2, space.Entities.Length);
        Assert.Equal(entity, space.Entities[1]);
    }

    /// <summary>
    /// Test <see cref="Space.Add{T}(in uint, in T)"/>.
    /// </summary>
    [Fact]
    public void TestAdd()
    {
        var space = new Space();
        var entity = space.Create();

        space.Add(entity, new Position(3, 4, 5));

        Assert.True(space.Has<Position>(entity));
        Assert.False(space.Has<Velocity>(entity));
        Assert.NotNull(space.Archetypes[1]);
    }

    /// <summary>
    /// Test <see cref="Space.Remove{T}(in uint)"/>.
    /// </summary>
    [Fact]
    public void TestRemove()
    {
        var space = new Space();
        var entity = space.Create();

        space.Add(entity, new Position(3, 4, 5));

        Assert.True(space.Has<Position>(entity));

        space.Remove<Position>(entity);

        Assert.False(space.Has<Position>(entity));
        Assert.Null(space.Archetypes[1]);
    }

    /// <summary>
    /// Test <see cref="Space.Has{T}(in uint)"/>.
    /// </summary>
    [Fact]
    public void TestHas()
    {
        var space = new Space();
        var entity = space.Create();

        space.Add(entity, new Position(3, 4, 5));

        Assert.True(space.Has<Position>(entity));
        Assert.False(space.Has<Velocity>(entity));
    }

    /// <summary>
    /// Test <see cref="Space.Get{T}(in uint)"/>.
    /// </summary>
    [Fact]
    public void TestGet()
    {
        var space = new Space();
        var entity = space.Create<Position>();
        var component = new Position(3, 4, 5);

        Assert.Throws<InvalidOperationException>(() => space.Get<Velocity>(entity));

        space.Set(entity, component);

        var position = space.Get<Position>(entity);

        Assert.Equal(component, position);
    }

    /// <summary>
    /// Test <see cref="Space.Set{T}(in uint, in T)"/>.
    /// </summary>
    [Fact]
    public void TestSet()
    {
        var space = new Space();
        var entity = space.Create<Position>();
        var component = new Position(3, 4, 5);

        Assert.Throws<InvalidOperationException>(() => space.Set(entity, new Velocity(1, 1, 1)));

        space.Set(entity, component);

        var position = space.Get<Position>(entity);

        Assert.Equal(component, position);
    }

    /// <summary>
    /// Test <see cref="Space.Destroy(in uint)"/>.
    /// </summary>
    [Fact]
    public void TestDestroyEntity()
    {
        var space = new Space();
        var entity = space.Create();

        space.Destroy(entity.Id);

        Assert.Equal((uint) 0, space.Archetypes[0]!.Chunks[0].Count);
    }

    /// <summary>
    /// Test <see cref="Space.Dispose"/>.
    /// </summary>
    [Fact]
    public void TestDispose()
    {
        new Space().Dispose();
    }
}