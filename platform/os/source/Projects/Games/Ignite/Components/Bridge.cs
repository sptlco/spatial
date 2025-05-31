// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// A <see cref="IComponent"/> exposing an <see cref="Entity"/> to the public network.
/// </summary>
/// <param name="Session">The <see cref="Models.Session"/> associated with the <see cref="Entity"/>.</param>
public record struct Bridge(ushort Session) : IComponent;