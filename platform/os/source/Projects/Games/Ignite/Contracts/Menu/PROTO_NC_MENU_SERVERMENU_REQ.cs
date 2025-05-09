// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Menu;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_MENU_SERVERMENU_REQ"/>.
/// </summary>
public class PROTO_NC_MENU_SERVERMENU_REQ : ProtocolBuffer
{
    /// <summary>
    /// The menu's title.
    /// </summary>
    public string title;

    /// <summary>
    /// The menu's priority level.
    /// </summary>
    public byte priority;

    /// <summary>
    /// The NPC that prompted the menu.
    /// </summary>
    public ushort npcHandle;

    /// <summary>
    /// The NPC's position.
    /// </summary>
    public SHINE_XY_TYPE npcPosition;

    /// <summary>
    /// How far away the player can move from the NPC until the menu closes.
    /// </summary>
    public ushort limitRange;

    /// <summary>
    /// The number of menu items.
    /// </summary>
    public byte menunum;

    /// <summary>
    /// Menu items.
    /// </summary>
    public SERVERMENU[] menu;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        title = ReadString(128);
        priority = ReadByte();
        npcHandle = ReadUInt16();
        npcPosition = Read<SHINE_XY_TYPE>();
        limitRange = ReadUInt16();
        menunum = ReadByte();
        menu = Read<SERVERMENU>(menunum);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(title, 128);
        Write(priority);
        Write(npcHandle);
        Write(npcPosition);
        Write(limitRange);
        Write(menunum);
        Write(menu);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        npcPosition.Dispose();

        foreach (var item in menu)
        {
            item.Dispose();
        }

        base.Dispose();
    }
}