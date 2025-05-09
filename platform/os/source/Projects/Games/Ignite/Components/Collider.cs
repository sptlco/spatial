// Copyright © Spatial. All rights reserved.

using Ignite.Models;
using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// An <see cref="Object"/> capable of collision with other objects.
/// </summary>
/// <param name="Size">The size of the <see cref="Object"/>.</param>
public record struct Collider(float Size) : IComponent;