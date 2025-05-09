// Copyright © Spatial. All rights reserved.

namespace Spatial.Simulation.Components;

/// <summary>
/// The velocity of an <see cref="Entity"/>.
/// </summary>
public record struct Velocity(float X, float Y, float Z) : IComponent;
