// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// Indicates that a <see cref="Player"/> has been modified and should be saved.
/// </summary>
public record struct Dirty : IComponent;