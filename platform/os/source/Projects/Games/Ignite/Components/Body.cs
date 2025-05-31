// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;
using Ignite.Models;
using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// A physical <see cref="Entity"/> in the <see cref="World"/>.
/// </summary>
/// <param name="Type">The body's <see cref="BodyType"/>.</param>
/// <param name="Field">The <see cref="Assets.Types.Field"/> in which the <see cref="Body"/> exists.</param>
/// <param name="Map">The <see cref="Models.Map"/> in which the <see cref="Body"/> exists.</param>
public record struct Body(
    BodyType Type,
    byte Field,
    int Map = 0) : IComponent;