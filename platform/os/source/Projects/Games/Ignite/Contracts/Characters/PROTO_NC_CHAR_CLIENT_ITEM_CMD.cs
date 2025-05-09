// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_CLIENT_ITEM_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_CLIENT_ITEM_CMD : ProtocolBuffer
{
    /// <summary>
    /// The number of items.
    /// </summary>
    public byte numofitem;

    /// <summary>
    /// The inventory type.
    /// </summary>
    public byte box;

    /// <summary>
    /// An inventory flag.
    /// </summary>
    public bool invenclear;

    /// <summary>
    /// Inventory items.
    /// </summary>
    public PROTO_ITEMPACKET_INFORM[] ItemArray;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        numofitem = ReadByte();
        box = ReadByte();
        invenclear = ReadBoolean();
        ItemArray = Read<PROTO_ITEMPACKET_INFORM>(numofitem);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(numofitem);
        Write(box);
        Write(invenclear);
        Write(ItemArray);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var item in ItemArray)
        {
            item.Dispose();
        }

        base.Dispose();
    }
}
