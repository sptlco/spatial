// Copyright © Spatial Corporation. All rights reserved.

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
    [Trait("Category", "Unit")]
    public void TestGetComponentId()
    {
        Assert.Equal(0, ComponentRegistry.GetComponentId<Position>());
    }

    /// <summary>
    /// Test <see cref="ComponentRegistry.GetComponentType"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestGetComponentType()
    {
        Assert.Equal(typeof(Position), ComponentRegistry.GetComponentType(0));
    }
}