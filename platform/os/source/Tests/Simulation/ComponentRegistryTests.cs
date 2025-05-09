// Copyright © Spatial. All rights reserved.

using Spatial.Simulation.Components;

namespace Spatial.Simulation;

/// <summary>
/// Unit tests for <see cref="ComponentRegistry"/>.
/// </summary>
public class ComponentRegistryTests
{
    /// <summary>
    /// Test <see cref="ComponentRegistry.GetComponentId{T}"/>.
    /// </summary>
    [Fact]
    public void TestGetComponentId()
    {
        Assert.Equal(0, ComponentRegistry.GetComponentId<Position>());
    }

    /// <summary>
    /// Test <see cref="ComponentRegistry.GetComponentType"/>.
    /// </summary>
    [Fact]
    public void TestGetComponentType()
    {
        Assert.Equal(typeof(Position), ComponentRegistry.GetComponentType(0));
    }
}