// Copyright © Spatial. All rights reserved.

using Ignite.Models;
using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing inventory data for an <see cref="Item"/>.
/// </summary>
public class ITEM_INVEN : ProtocolBuffer
{
    /// <summary>
    /// The item's inventory slot.
    /// </summary>
    public ushort Pos;

    /// <summary>
    /// The type of inventory the item is stored in.
    /// </summary>
    public ushort Type;

    /// <summary>
    /// Create a new <see cref="ITEM_INVEN"/>.
    /// </summary>
    public ITEM_INVEN() { }

    /// <summary>
    /// Create a new <see cref="ITEM_INVEN"/>.
    /// </summary>
    /// <param name="slot">An <see cref="Inventory"/> slot.</param>
    /// <param name="inventory">The inventory's <see cref="InventoryType"/>.</param>
    public ITEM_INVEN(byte slot, InventoryType inventory)
    {
        Pos = slot;
        Type = (byte) inventory;
    }

    /// <summary>
    /// Create a new <see cref="ITEM_INVEN"/>.
    /// </summary>
    /// <param name="item">An <see cref="Item"/>.</param>
    public ITEM_INVEN(Item item)
    {
        Pos = item.Slot;
        Type = (ushort) item.Inventory;
    }

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        var inventory = ReadUInt16();

        Pos = (ushort) (inventory & 0x3FF);
        Type = (ushort) ((inventory >> 10) & 0x3F);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write((ushort) (((Type & 0x3F) << 10) | (Pos & 0x3FF)));
    }
}
