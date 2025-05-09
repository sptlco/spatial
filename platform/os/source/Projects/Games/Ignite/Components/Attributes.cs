// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// Core attributes influencing an object.
/// </summary>
/// <param name="Strength">The object's strength.</param>
/// <param name="Endurance">The object's endurance.</param>
/// <param name="Dexterity">The object's dexterity.</param>
/// <param name="Intelligence">The object's intelligence.</param>
/// <param name="Wisdom">The object's wisdom.</param>
/// <param name="Spirit">The object's spirit.</param>
public record struct Attributes(
    float Strength = 0F,
    float Endurance = 0F,
    float Dexterity = 0F,
    float Intelligence = 0F,
    float Wisdom = 0F,
    float Spirit = 0F) : IComponent;