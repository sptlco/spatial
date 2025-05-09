// Copyright © Spatial. All rights reserved.

using Ignite.Models;
using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing packet data.
/// </summary>
public class PROTO_ITEMPACKET_INFORM : ProtocolBuffer
{
    /// <summary>
    /// Create a new <see cref="PROTO_ITEMPACKET_INFORM"/>.
    /// </summary>
    public PROTO_ITEMPACKET_INFORM() { }

    /// <summary>
    /// Create a new <see cref="PROTO_ITEMPACKET_INFORM"/>.
    /// </summary>
    /// <param name="item">An <see cref="Item"/>.</param>
    public PROTO_ITEMPACKET_INFORM(Item item)
    {
        location = new ITEM_INVEN(item);
        info = new SHINE_ITEM_STRUCT(item);
    }

    /// <summary>
    /// The size of the item's buffer.
    /// </summary>
    public byte datasize;

    /// <summary>
    /// The item's location.
    /// </summary>
    public ITEM_INVEN location;

    /// <summary>
    /// The item's information.
    /// </summary>
    public SHINE_ITEM_STRUCT info;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        datasize = ReadByte();
        location = Read<ITEM_INVEN>();
        info = Read<SHINE_ITEM_STRUCT>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        info.Serialize();

        Write((byte) info.Size);
        Write(location);
        Write(info.ToArray());
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        location.Dispose();
        info.Dispose();

        base.Dispose();
    }
}
