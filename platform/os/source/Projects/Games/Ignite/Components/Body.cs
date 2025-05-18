// Copyright © Spatial. All rights reserved.

using Ignite.Models;
using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// The physical apperatus of an <see cref="Object"/>.
/// </summary>
/// <param name="Size">The size of the <see cref="Body"/>.</param>
public record struct Body(float Size = 0F) : IComponent;