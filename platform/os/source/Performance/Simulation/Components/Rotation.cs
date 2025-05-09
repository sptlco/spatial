// Copyright © Spatial. All rights reserved.

namespace Spatial.Simulation.Components;

/// <summary>
/// The rotation of an <see cref="Entity"/>.
/// </summary>
public record struct Rotation(float Degrees) : IComponent;