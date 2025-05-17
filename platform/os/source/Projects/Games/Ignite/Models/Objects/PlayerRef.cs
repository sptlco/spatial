// Copyright © Spatial. All rights reserved.

using Ignite.Assets.Types;
using Ignite.Components;
using Ignite.Contracts;
using Ignite.Contracts.Actions;
using Ignite.Contracts.Maps;
using Ignite.Contracts.Menu;
using Ignite.Contracts.Objects;
using Spatial.Extensions;
using Spatial.Mathematics;
using Spatial.Simulation;

namespace Ignite.Models.Objects;

/// <summary>
/// A reference to a player <see cref="Object"/>.
/// </summary>
public class PlayerRef : Object
{
    /// <summary>
    /// Create a new <see cref="PlayerRef"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> the <see cref="Object"/> is in.</param>
    /// <param name="entity">The <see cref="Entity"/> to reference.</param>
    public PlayerRef(Map map, Entity entity) : base(map, entity) { }

    /// <summary>
    /// The referenced <see cref="Player"/>.
    /// </summary>
    public Player Object => Get<Player>();

    /// <summary>
    /// The player's <see cref="Models.Session"/>.
    /// </summary>
    public Session Session => Session.Find(Object.Session);

    /// <summary>
    /// The player's <see cref="Models.Character"/>.
    /// </summary>
    public Character Character => Session.Character;

    /// <summary>
    /// Interact with an <see cref="NPCRef"/>.
    /// </summary>
    /// <param name="npc">An <see cref="NPCRef"/>.</param>
    public void Interact(NPCRef npc)
    {
        if (npc.Object.Menu)
        {
            World.Command(
                connection: Session.Map,
                command: NETCOMMAND.NC_ACT_NPCMENUOPEN_REQ,
                data: new PROTO_NC_ACT_NPCMENUOPEN_REQ {
                    mobid = npc.Object.Id
                });

            return;
        }

        npc.Play(this);
    }

    /// <summary>
    /// Prompt the <see cref="PlayerRef"/>.
    /// </summary>
    /// <param name="priority">The priority of the message.</param>
    /// <param name="title">The title of the message.</param>
    /// <param name="range">The maximum distance between the <see cref="PlayerRef"/> and <paramref name="sender"/>.</param>
    /// <param name="sender">The <see cref="Models.Object"/> prompting the <see cref="PlayerRef"/>.</param>
    /// <param name="items">A list of items the <see cref="PlayerRef"/> can choose from.</param>
    public void Prompt(
        byte priority,
        string title,
        float range,
        Object? sender = null,
        params Menu.Item[] items)
    {
        Session.Callbacks.Clear();

        World.Command(
            connection: Session.Map,
            command: NETCOMMAND.NC_MENU_SERVERMENU_REQ,
            data: new PROTO_NC_MENU_SERVERMENU_REQ {
                title = title,
                priority = priority,
                npcHandle = sender?.Tag.Handle ?? ushort.MaxValue,
                npcPosition = new SHINE_XY_TYPE {
                    x = (uint) (sender?.Transform ?? Transform).X,
                    y = (uint) (sender?.Transform ?? Transform).Y
                },
                limitRange = (ushort) range,
                menunum = (byte) items.Length,
                menu = items.ToArray(item => new SERVERMENU {
                    reply = (byte) Session.Callbacks.Add(item.Function),
                    str = item.Title
                })
            });
    }

    /// <summary>
    /// Teleport the <see cref="PlayerRef"/> to a location.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> to teleport the <see cref="PlayerRef"/> to.</param>
    /// <param name="id">The map's identification number.</param>
    /// <param name="transform">The <see cref="Transform"/> to teleport the <see cref="PlayerRef"/> to.</param>
    public void Teleport(byte map, int id, Transform? transform = null)
    {
        var destination = Map.InstanceAtOrDefault(map, id) ?? Map.Load(Field.Find(map), id);

        transform ??= new Transform(X: destination.Data.Info.RegenX, Y: destination.Data.Info.RegenY);

        var session = Session;
        var character = Character;

        Release();

        character.Update(c => {
            c.Map = map;
            c.Position = new Point2D(X: transform.Value.X, Y: transform.Value.Y);
            c.Rotation = transform.Value.R;
        });

        _map = destination;
        _entity = Player.Create(session, character, _map).UID;

        Session.Object = this;

        World.Command(
            connection: Session.Map,
            command: NETCOMMAND.NC_MAP_LINKSAME_CMD,
            data: new PROTO_NC_MAP_LINKSAME_CMD {
                mapid = destination.Data.Info.ID,
                location = new SHINE_XY_TYPE {
                    x = (uint) transform.Value.X,
                    y = (uint) transform.Value.Y
                }
            });
    }

    /// <summary>
    /// Enter another <see cref="Models.Object"/>.
    /// </summary>
    /// <param name="other">Another <see cref="Models.Object"/>.</param>
    public override void Enter(Object other)
    {
        if (other is NPCRef npc && npc.Gate is not null)
        {
            Interact(npc);
        }
    }

    /// <summary>
    /// Focus another <see cref="Models.Object"/>.
    /// </summary>
    /// <param name="other">Another <see cref="Models.Object"/>.</param>
    public override void Focus(Object other)
    {
        switch (other)
        {
            case MobRef:
            case NPCRef:
                World.Command(
                    connection: Session.Map,
                    command: NETCOMMAND.NC_BRIEFINFO_REGENMOB_CMD,
                    data: new PROTO_NC_BRIEFINFO_REGENMOB_CMD(other));
                    
                break;
        }
    }

    /// <summary>
    /// Blur another <see cref="Models.Object"/>.
    /// </summary>
    /// <param name="other">Another <see cref="Models.Object"/>.</param>
    public override void Blur(Object other)
    {
        if (other is not NPCRef)
        {
            World.Command(
                connection: Session.Map,
                command: NETCOMMAND.NC_BRIEFINFO_BRIEFINFODELETE_CMD,
                data: new PROTO_NC_BRIEFINFO_BRIEFINFODELETE_CMD {
                    hnd = other.Tag.Handle
                });
        }
    }

    /// <summary>
    /// Convert the <see cref="PlayerRef"/> to a <see cref="string"/>.
    /// </summary>
    /// <returns>A <see cref="string"/>.</returns>
    public override string ToString()
    {
        return Character.Name;
    }
}