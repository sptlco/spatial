// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// A pre-defined set of behaviors.
/// </summary>
/// <param name="Id">The script's identification number.</param>
public record struct Script(ushort Id) : IComponent;