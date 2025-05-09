// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// An object's core vitals.
/// </summary>
/// <param name="Level">The object's level.</param>
/// <param name="Health">The object's health.</param>
/// <param name="Spirit">The object's spirit.</param>
/// <param name="Light">The object's light.</param>
public record struct Vitals(
    byte Level,
    Parameter Health = new(),
    Parameter Spirit = new(),
    Parameter Light = new()) : IComponent;