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
/// <param name="Saved">The last time the player was saved.</param>
public record struct Player(
    ushort Session,
    double Saved = 0
    ) : IComponent
{
    /// <summary>
    /// Create a new <see cref="Player"/>.
    /// </summary>
    /// <param name="session">The player's <see cref="Models.Session"/>.</param>
    /// <param name="character">The player's <see cref="Character"/>.</param>
    /// <param name="map">The <see cref="Map"/> to create the <see cref="Player"/> in.</param>
    /// <returns>A <see cref="Player"/>.</returns>
    public static PlayerRef Reference(Session session, Map map)
    {
        var parameters = Param.Stats(session.Character.Class, session.Character.Level);

        var player = map.CreateObject<PlayerRef>(
            type: ObjectType.Player,
            attributes: new Attributes(Strength: parameters.Strength, Endurance: parameters.Constitution, Dexterity: parameters.Dexterity, Intelligence: parameters.Intelligence, Wisdom: parameters.Wizdom, Spirit: parameters.MentalPower),
            vitals: new Vitals(Level: session.Character.Level, Health: new Parameter(Maximum: parameters.MaxHP, Current: session.Character.HP)),
            transform: new Transform(X: session.Character.Position.X, Y: session.Character.Position.Y),
            speed: new Speed(Walking: Common.WalkSpeed, Running: Common.RunSpeed, Attacking: Common.AttackSpeed));

        if (session.Character.Class >= Class.Crusader)
        {
            player.Vitals.Light = new Parameter(Maximum: SingleData.MaxLP, Current: session.Character.LP);
        }
        else
        {
            player.Vitals.Spirit = new Parameter(Maximum: parameters.MaxSP, Current: session.Character.SP);
        }

        player
            .Add(new Player(Session: session.Handle, Saved: World.Time))
            .Add(new Body(Size: 10.0F))
            .Add(new Abilities(player))
            .Add(new Stones(
                Health: new Parameter(Maximum: parameters.MAXSoulHP, Current: session.Character.HPStones),
                Spirit: new Parameter(Maximum: parameters.MAXSoulSP, Current: session.Character.SPStones)));

        return player;
    }
}