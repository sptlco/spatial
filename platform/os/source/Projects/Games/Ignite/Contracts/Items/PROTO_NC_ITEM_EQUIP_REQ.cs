// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Items;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_ITEM_EQUIP_REQ"/>.
/// </summary>
public class PROTO_NC_ITEM_EQUIP_REQ : ProtocolBuffer
{
    /// <summary>
    /// The item's slot.
    /// </summary>
    public byte slot;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        slot = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(slot);
    }
}
