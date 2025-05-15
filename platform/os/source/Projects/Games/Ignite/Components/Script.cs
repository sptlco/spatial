// Copyright © Spatial. All rights reserved.

using Ignite.Models;
using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// A pre-defined set of behaviors for an <see cref="Object"/>.
/// </summary>
/// <param name="Id">The script's identification number.</param>
public record struct Script(ushort Id) : IComponent;