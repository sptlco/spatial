// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Simulation;

/// <summary>
/// A group of entities.
/// </summary>
/// <param name="Id">The group's identification number.</param>
public record struct Group(uint Id) : IComponent;