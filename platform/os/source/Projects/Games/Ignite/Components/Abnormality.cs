// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// Temporarily modified state of an object.
/// </summary>
/// <param name="Object">The object affected by the <see cref="Abnormality"/>.</param>
public record struct Abnormality(uint Object) : IComponent;