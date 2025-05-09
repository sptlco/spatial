// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// Magical stones with healing powers.
/// </summary>
/// <param name="Health">The object's health stones.</param>
/// <param name="Spirit">The object's spirit stones.</param>
public record struct Stones(
    Parameter Health = new(),
    Parameter Spirit = new()) : IComponent;