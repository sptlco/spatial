// Copyright © Spatial. All rights reserved.

using Ignite.Assets.Types;
using Ignite.Contracts;
using Ignite.Models;
using Ignite.Models.Objects;
using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// A player-controller object.
/// </summary>
/// <param name="Session">The player's session identification number.</param>
/// <param name="Character">The player's character identification number.</param>
public record struct Player(ushort Session, uint Character) : IComponent
{
    /// <summary>
    /// Create a new <see cref="Player"/>.
    /// </summary>
    /// <param name="session">The player's <see cref="Models.Session"/>.</param>
    /// <param name="character">The player's <see cref="Models.Character"/>.</param>
    /// <param name="map">The <see cref="Map"/> to create the <see cref="Player"/> in.</param>
    /// <returns>A <see cref="Player"/>.</returns>
    public static PlayerRef Create(Session session, Character character, Map map)
    {
        var parameters = Param.Stats(character.Class, character.Level);

        var player = map.CreateObject<PlayerRef>(
            type: ObjectType.Player,
            attributes: new Attributes(Strength: parameters.Strength, Endurance: parameters.Constitution, Dexterity: parameters.Dexterity, Intelligence: parameters.Intelligence, Wisdom: parameters.Wizdom, Spirit: parameters.MentalPower),
            vitals: new Vitals(Level: character.Level, Health: new Parameter(Maximum: parameters.MaxHP, Current: character.HP)),
            transform: new Transform(X: character.Position.X, Y: character.Position.Y, R: character.Rotation),
            speed: new Speed(Walking: Common.WalkSpeed, Running: Common.RunSpeed, Attacking: Common.AttackSpeed));

        if (character.Class >= Class.Crusader)
        {
            player.Vitals.Light = new Parameter(Maximum: SingleData.MaxLP, Current: character.LP);
        }
        else
        {
            player.Vitals.Spirit = new Parameter(Maximum: parameters.MaxSP, Current: character.SP);
        }

        player
            .Add(new Player(Session: session.Handle, Character: character.Id))
            .Add(new Body(Size: 10.0F))
            .Add(new Abilities(player))
            .Add(new Stones(
                Health: new Parameter(Maximum: parameters.MAXSoulHP, Current: character.HPStones),
                Spirit: new Parameter(Maximum: parameters.MAXSoulSP, Current: character.SPStones)));

        return player;
    }
}