// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// A monster inhabiting the <see cref="Models.World"/>.
/// </summary>
/// <param name="Id">The mob's identification number.</param>
public record struct Mob(ushort Id) : IComponent;