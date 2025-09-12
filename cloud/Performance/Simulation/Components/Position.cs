// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Simulation.Components;

/// <summary>
/// The position of an <see cref="Entity"/>
/// </summary>
/// <param name="X">The entity's X-coordinate.</param>
/// <param name="Y">The entity's Y-coordinate.</param>
/// <param name="Z">The entity's Z-coordinate.</param>
public record struct Position(float X, float Y, float Z) : IComponent;