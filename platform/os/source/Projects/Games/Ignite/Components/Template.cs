// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// A <see cref="IComponent"/> with common data for a <see cref="Body"/>.
/// </summary>
/// <param name="Id">The template's identification number.</param>
public record struct Template(uint Id) : IComponent;