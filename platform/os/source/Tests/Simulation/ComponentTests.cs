// Copyright © Spatial. All rights reserved.

using Spatial.Simulation.Components;

namespace Spatial.Simulation;

/// <summary>
/// Unit tests for <see cref="Component{T}"/>.
/// </summary>
public class ComponentTests
{
    /// <summary>
    /// Test <see cref="Component{T}.Id"/>.
    /// </summary>
    [Fact]
    public void TestId()
    {
        Assert.Equal(0, Component<Position>.Id);
        Assert.Equal(1, Component<Velocity>.Id);
    }

    /// <summary>
    /// Test <see cref="Component{T}.Size"/>.
    /// </summary>
    [Fact]
    public void TestSize()
    {
        Assert.Equal(12, Component<Position>.Size);
        Assert.Equal(12, Component<Velocity>.Size);
    }
}
