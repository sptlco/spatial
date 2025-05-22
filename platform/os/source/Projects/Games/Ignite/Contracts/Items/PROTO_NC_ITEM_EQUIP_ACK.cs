// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Items;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_ITEM_EQUIP_ACK"/>.
/// </summary>
public class PROTO_NC_ITEM_EQUIP_ACK : ProtocolBuffer
{
    /// <summary>
    /// An error that occurred.
    /// </summary>
    public ushort error;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        error = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(error);
    }
}
