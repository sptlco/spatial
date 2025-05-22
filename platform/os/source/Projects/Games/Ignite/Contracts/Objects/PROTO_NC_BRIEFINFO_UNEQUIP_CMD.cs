// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Objects;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_BRIEFINFO_UNEQUIP_CMD"/>.
/// </summary>
public class PROTO_NC_BRIEFINFO_UNEQUIP_CMD : ProtocolBuffer
{
    /// <summary>
    /// The related object.
    /// </summary>
    public ushort handle;

    /// <summary>
    /// The equipment slot.
    /// </summary>
    public byte slot;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        handle = ReadUInt16();
        slot = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(handle);
        Write(slot);
    }
}
