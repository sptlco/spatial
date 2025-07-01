// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Simulation.Components;

/// <summary>
/// The position of an <see cref="Entity"/>.
/// </summary>
public record struct Position(float X, float Y, float Z) : IComponent;