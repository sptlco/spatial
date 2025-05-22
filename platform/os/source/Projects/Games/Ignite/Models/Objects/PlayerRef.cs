// Copyright © Spatial. All rights reserved.

using Ignite.Assets;
using Ignite.Assets.Types;
using Ignite.Components;
using Ignite.Contracts;
using Ignite.Contracts.Actions;
using Ignite.Contracts.Items;
using Ignite.Contracts.Maps;
using Ignite.Contracts.Menu;
using Ignite.Contracts.Objects;
using Spatial.Extensions;
using Spatial.Simulation;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ignite.Models.Objects;

/// <summary>
/// A reference to a player <see cref="Value"/>.
/// </summary>
public class PlayerRef : ObjectRef
{
    /// <summary>
    /// Create a new <see cref="PlayerRef"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> the <see cref="Value"/> is in.</param>
    /// <param name="entity">The <see cref="Entity"/> to reference.</param>
    public PlayerRef(Map map, Entity entity) : base(map, entity) { }

    /// <summary>
    /// The referenced <see cref="Player"/>.
    /// </summary>
    public Player Value => Get<Player>();

    /// <summary>
    /// The player's <see cref="Models.Session"/>.
    /// </summary>
    public Session Session => Session.Find(Value.Session);

    /// <summary>
    /// The player's <see cref="Models.Character"/>.
    /// </summary>
    public Character Character => Session.Character;

    /// <summary>
    /// The player's name.
    /// </summary>
    public new string Name => Character.Name;

    /// <summary>
    /// Interact with an <see cref="NPCRef"/>.
    /// </summary>
    /// <param name="npc">An <see cref="NPCRef"/>.</param>
    public void Interact(NPCRef npc)
    {
        if (npc.Value.Menu)
        {
            Session.ToMap(NETCOMMAND.NC_ACT_NPCMENUOPEN_REQ, new PROTO_NC_ACT_NPCMENUOPEN_REQ {
                mobid = npc.Value.Id
            });

            return;
        }

        switch (npc.Value.Role)
        {
            case NPCRole.Gate when npc.Gate is Gate gate:
                var field = Field.Find(gate.Map);
                var map = Asset.First<MapInfo>("MapInfo.shn", m => m.MapName == field.MapIDClient);

                Prompt(
                    priority: 0,
                    title: Assets.Types.Script.String("MenuString", "LinkTitle", map.Name),
                    range: 1000F,
                    sender: this,
                    items: [
                        new(Assets.Types.Script.String("ETC", "Yes"), () => Teleport(gate.Map, gate.Id, new Transform(gate.X, gate.Y, gate.R))),
                        new(Assets.Types.Script.String("ETC", "No"), () => { }),
                    ]);

                break;
        }
    }

    /// <summary>
    /// Notify the <see cref="PlayerRef"/> of something.
    /// </summary>
    /// <param name="message">The notification's message.</param>
    public void Notify(string message)
    {
        var bytes = Encoding.ASCII.GetBytes(message);

        Session.ToMap(NETCOMMAND.NC_ACT_NOTICE_CMD, new PROTO_NC_ACT_NOTICE_CND {
            len = (byte) bytes.Length,
            content = bytes
        });
    }

    /// <summary>
    /// Prompt the <see cref="PlayerRef"/>.
    /// </summary>
    /// <param name="priority">The priority of the message.</param>
    /// <param name="title">The title of the message.</param>
    /// <param name="range">The maximum distance between the <see cref="PlayerRef"/> and <paramref name="sender"/>.</param>
    /// <param name="sender">The <see cref="ObjectRef"/> prompting the <see cref="PlayerRef"/>.</param>
    /// <param name="items">A list of items the <see cref="PlayerRef"/> can choose from.</param>
    public void Prompt(
        byte priority,
        string title,
        float range,
        ObjectRef? sender = null,
        params Menu.Item[] items)
    {
        Session.Callbacks.Clear();

        Session.ToMap(NETCOMMAND.NC_MENU_SERVERMENU_REQ, new PROTO_NC_MENU_SERVERMENU_REQ {
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
    /// <param name="x">An X-coordinate.</param>
    /// <param name="y">A Y-coordinate.</param>
    public void Teleport(in byte map, in int id, in float x, in float y)
    {
        Teleport(map, id, new Transform(x, y));
    }

    /// <summary>
    /// Teleport the <see cref="PlayerRef"/> to a location.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> to teleport the <see cref="PlayerRef"/> to.</param>
    /// <param name="id">The map's identification number.</param>
    /// <param name="transform">The <see cref="Transform"/> to teleport the <see cref="PlayerRef"/> to.</param>
    public void Teleport(byte map, in int id, Transform? transform = null)
    {
        var destination = Map.InstanceAtOrDefault(map, id) ?? Map.Load(Field.Find(map), id);

        transform ??= new Transform(X: destination.Data.Info.RegenX, Y: destination.Data.Info.RegenY);

        var session = Session;
        var character = Character;

        Release();

        character.Update(c => {
            c.Map = map;
            c.Position = transform.Value;
        });

        _map = destination;
        _entity = Player.CreateRef(session, _map).UID;

        Session.Player = this;

        Session.ToMap(NETCOMMAND.NC_MAP_LINKSAME_CMD, new PROTO_NC_MAP_LINKSAME_CMD {
            mapid = destination.Data.Info.ID,
            location = new SHINE_XY_TYPE {
                x = (uint) transform.Value.X,
                y = (uint) transform.Value.Y
            }
        });
    }

    /// <summary>
    /// Equip an <see cref="Item"/>.
    /// </summary>
    /// <param name="item">The <see cref="Item"/> to equip.</param>
    public void Equip(Item item)
    {
        var from = item.Slot;
        var to = (byte) item.Data.Client.Equip;

        // Get the item that is currently equipped in the target slot.
        // If the item exists, we should swap it with the item being equipped.

        var piece = Character.Equipment.ItemAtOrDefault(to);

        if (piece is not null)
        {
            // As mentioned above, we swap the existing piece with the new piece.

            piece.Inventory = item.Inventory;
            piece.Slot = from;
        }

        item.Inventory = InventoryType.Equipment;
        item.Slot = to;

        // Whether there was an existing piece or not, the original inventory 
        // slot will be updated accordingly (to either null or the old piece).

        Character.Inventory[from] = piece;
        Character.Equipment[item.Slot] = item;

        // Save both items, and ensure the client's inventory state is 
        // properly updated after equipment.

        item.Save();
        piece?.Save();

        Session.ToMap(
            command: NETCOMMAND.NC_ITEM_EQUIPCHANGE_CMD,
            data: new PROTO_NC_ITEM_EQUIPCHANGE_CMD {
                exchange = new ITEM_INVEN(from, Character.Inventory.Type),
                location = item.Slot,
                item = new SHINE_ITEM_STRUCT(item)
            });

        Session.ToMap(
            command: NETCOMMAND.NC_ITEM_CELLCHANGE_CMD,
            data: new PROTO_NC_ITEM_CELLCHANGE_CMD {
                exchange = new ITEM_INVEN(to, Character.Equipment.Type),
                location = new ITEM_INVEN(from, Character.Inventory.Type),
                item = new SHINE_ITEM_STRUCT(piece)
            });

        Session.ToMap(
            command: NETCOMMAND.NC_ITEM_EQUIP_ACK,
            data: new PROTO_NC_ITEM_EQUIP_ACK {
                // ...
            });

        // Broadcast the equipment change to surrounding objects.
        // Determine which type of message to broadcast based on the item's type.

        switch (item.Data.Client.Class)
        {
            case ItemClassEnum.ITEMCLASS_WEAPON:
                var title = Asset
                    .View<WeaponTitleData>("WeaponTitleData.shn", t => t.MobID == item.TitleMobId && item.Kills.GetValueOrDefault(t.MobID) >= t.MobKillCount)
                    .OrderBy(t => t.MobKillCount)
                    .LastOrDefault();

                Map.MulticastExclusive2D(
                    command: NETCOMMAND.NC_BRIEFINFO_CHANGEWEAPON_CMD,
                    position: Transform,
                    exclude: [Tag.Handle],
                    data: new PROTO_NC_BRIEFINFO_CHANGEWEAPON_CMD {
                        upgradeinfo = new PROTO_NC_BRIEFINFO_CHANGEUPGRADE_CMD {
                            handle = Tag.Handle,
                            item = item.ItemId,
                            upgrade = item.Upgrades,
                            nSlotNum = item.Slot
                        },
                        currentmobid = title?.MobID ?? ushort.MaxValue,
                        currentkilllevel = title?.Level ?? 0
                    });

                break;
            case ItemClassEnum.ITEMCLASS_SHIELD:
            case ItemClassEnum.ITEMCLASS_ARMOR:
            case ItemClassEnum.ITEMCLASS_BOOT:
            case ItemClassEnum.ITEMCLASS_AMULET:
            case ItemClassEnum.ITEMCLASS_BRACELET:
                Map.MulticastExclusive2D(
                    command: NETCOMMAND.NC_BRIEFINFO_CHANGEUPGRADE_CMD,
                    position: Transform,
                    exclude: [Tag.Handle],
                    data: new PROTO_NC_BRIEFINFO_CHANGEUPGRADE_CMD {
                        handle = Tag.Handle,
                        item = item.ItemId,
                        upgrade = item.Upgrades,
                        nSlotNum = item.Slot
                    });

                break;
            default:
                Map.MulticastExclusive2D(
                    command: NETCOMMAND.NC_BRIEFINFO_CHANGEDECORATE_CMD,
                    position: Transform,
                    exclude: [Tag.Handle],
                    data: new PROTO_NC_BRIEFINFO_CHANGEDECORATE_CMD {
                        handle = Tag.Handle,
                        item = item.ItemId,
                        nSlotNum = item.Slot
                    });

                break;
        }
    }

    /// <summary>
    /// Enter another <see cref="ObjectRef"/>.
    /// </summary>
    /// <param name="other">Another <see cref="ObjectRef"/>.</param>
    public override void Enter(ObjectRef other)
    {
        if (other is NPCRef npc && npc.Gate is not null)
        {
            Interact(npc);
        }
    }

    /// <summary>
    /// Focus another <see cref="ObjectRef"/>.
    /// </summary>
    /// <param name="other">Another <see cref="ObjectRef"/>.</param>
    public override void Focus(ObjectRef other)
    {
        switch (other)
        {
            case MobRef:
            case NPCRef:
                Session.ToMap(NETCOMMAND.NC_BRIEFINFO_REGENMOB_CMD, new PROTO_NC_BRIEFINFO_REGENMOB_CMD(other));
                break;
            case PlayerRef player:
                Session.ToMap(NETCOMMAND.NC_BRIEFINFO_LOGINCHARACTER_CMD, new PROTO_NC_BRIEFINFO_LOGINCHARACTER_CMD(player));
                break;
        }
    }

    /// <summary>
    /// Blur another <see cref="ObjectRef"/>.
    /// </summary>
    /// <param name="other">Another <see cref="ObjectRef"/>.</param>
    public override void Blur(ObjectRef other)
    {
        if (other is not NPCRef)
        {
            Session.ToMap(NETCOMMAND.NC_BRIEFINFO_BRIEFINFODELETE_CMD, new PROTO_NC_BRIEFINFO_BRIEFINFODELETE_CMD {
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
        return Name;
    }
}