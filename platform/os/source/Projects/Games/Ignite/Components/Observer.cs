// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// An <see cref="Models.Objects.ObjectRef"/> observing an <see cref="Models.Objects.ObjectRef"/>.
/// </summary>
/// <param name="Object">The observing object's identification number.</param>
/// <param name="Target">The observed object's identification number.</param>
public record struct Observer(uint Object, uint Target) : IComponent;