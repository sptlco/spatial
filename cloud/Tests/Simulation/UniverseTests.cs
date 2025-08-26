// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Simulation.Systems;

namespace Spatial.Simulation;

/// <summary>
/// Unit tests for <see cref="Universe"/>.
/// </summary>
public class UniverseTests
{
    /// <summary>
    /// Test <see cref="Universe.Update"/>.
    /// </summary>
    [Fact]
    public void TestUpdate()
    {
        var counter = new Counter();
        var universe = Universe.Create();
        var space = Space.Empty().Use(counter);

        universe.Add(space);
        universe.Update(33);

        Assert.Equal(3, counter.Count);
    }

    /// <summary>
    /// Test <see cref="Universe.Dispose"/>.
    /// </summary>
    [Fact]
    public void TestDispose()
    {
        var universe = Universe.Create();

        universe.Add(Space.Empty());
        universe.Dispose();

        Assert.Empty(universe);
    }
}
