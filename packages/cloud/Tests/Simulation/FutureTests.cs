// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Simulation.Components;

namespace Spatial.Simulation;

/// <summary>
/// Unit tests for <see cref="Future"/>.
/// </summary>
public class FutureTests
{
    /// <summary>
    /// Test <see cref="Future.New"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestNew()
    {
        Future.New();
    }

    /// <summary>
    /// Test <see cref="Future.New"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestReserve()
    {
        var future = Future.New();

        future.Reserve(50);
        future.Reserve(Signature.Empty, 10);
    }

    /// <summary>
    /// Test <see cref="Future.Create"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestCreate()
    {
        var future = Future.New();

        future.Create();
        future.Create(Signature.Of<Position>());
    }

    /// <summary>
    /// Test <see cref="Future.Add"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestAdd()
    {
        Future.New().Add(506, new Position());
    }

    /// <summary>
    /// Test <see cref="Future.Remove"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestRemove()
    {
        Future.New().Remove<Velocity>(403);
    }

    /// <summary>
    /// Test <see cref="Future.Set"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestSet()
    {
        Future.New().Set(102, new Position());
    }

    /// <summary>
    /// Test <see cref="Future.Destroy"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestDestroy()
    {
        Future.New().Destroy(50);
    }

    /// <summary>
    /// Test <see cref="Future.Commit"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestCommit()
    {
        var space = Space.Empty();
        var future = Future.New();

        future.Create();
        future.Commit(space);
    }

    /// <summary>
    /// Test <see cref="Future.Dispose"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestDispose()
    {
        Future.New().Dispose();
    }
}