// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Items;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_ITEM_EQUIPCHANGE_CMD"/>.
/// </summary>
public class PROTO_NC_ITEM_EQUIPCHANGE_CMD : ProtocolBuffer
{
    /// <summary>
    /// The item that was exchanged for this item.
    /// </summary>
    public ITEM_INVEN exchange;

    /// <summary>
    /// The modified item's location.
    /// </summary>
    public byte location;

    /// <summary>
    /// The modified item.
    /// </summary>
    public SHINE_ITEM_STRUCT item;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        exchange = Read<ITEM_INVEN>();
        location = ReadByte();
        item = Read<SHINE_ITEM_STRUCT>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(exchange);
        Write(location);
        Write(item);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        exchange.Dispose();
        item.Dispose();

        base.Dispose();
    }
}
