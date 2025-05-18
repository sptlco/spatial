// Copyright © Spatial. All rights reserved.

using Ignite.Assets.Types;
using Ignite.Models.Objects;
using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// An object's combative abilities.
/// </summary>
/// <param name="Damage">The object's damage output.</param>
/// <param name="Defense">The object's defense.</param>
/// <param name="MagicDamage">The object's magic damage output.</param>
/// <param name="MagicDefense">The object's magic defense.</param>
/// <param name="Aim">The object's aim.</param>
/// <param name="Evasion">The object's evasion.</param>
public record struct Abilities(
    Range Damage = new Range(),
    float Defense = 0F,
    Range MagicDamage = new Range(),
    float MagicDefense = 0F,
    float Aim = 0F,
    float Evasion = 0F) : IComponent
{
    /// <summary>
    /// Create new <see cref="Abilities"/>.
    /// </summary>
    /// <param name="level">The object's level.</param>
    /// <param name="strength">The object's strength.</param>
    /// <param name="endurance">The object's endurance.</param>
    /// <param name="dexterity">The object's dexterity.</param>
    /// <param name="intelligence">The object's intelligence.</param>
    /// <param name="wisdom">The object's wisdom.</param>
    /// <param name="spirit">The object's spirit.</param>
    public Abilities(byte level, float strength, float endurance, float dexterity, float intelligence, float wisdom, float spirit) : this(
            Damage: new Range(Minimum: 0.0F, Maximum: 0.0F),
            Defense: 0.0F,
            MagicDamage: new Range(Minimum: 0.0F, Maximum: 0.0F),
            MagicDefense: 0.0F,
            Aim: 0.0F,
            Evasion: 0.0F)
    {
    }

    /// <summary>
    /// Compute an object's <see cref="Abilities"/>.
    /// </summary>
    /// <param name="object">The object whose abilities to compute.</param>
    /// <returns>The object's <see cref="Abilities"/>.</returns>
    public Abilities(ObjectRef @object) : this(
            level: @object.Vitals.Level,
            strength: @object.Attributes.Strength,
            endurance: @object.Attributes.Endurance,
            dexterity: @object.Attributes.Dexterity,
            intelligence: @object.Attributes.Intelligence,
            wisdom: @object.Attributes.Wisdom,
            spirit: @object.Attributes.Spirit)
    {
    }

    /// <summary>
    /// Compute an object's <see cref="Abilities"/>.
    /// </summary>
    /// <param name="parameters">The object's <see cref="Param"/>.</param>
    /// <returns>The object's <see cref="Abilities"/>.</returns>
    public Abilities(Param parameters) : this(
            level: (byte) parameters.Level,
            strength: parameters.Strength,
            endurance: parameters.Constitution,
            dexterity: parameters.Dexterity,
            intelligence: parameters.Intelligence,
            wisdom: parameters.Wizdom,
            spirit: parameters.MentalPower)
    {
    }
}