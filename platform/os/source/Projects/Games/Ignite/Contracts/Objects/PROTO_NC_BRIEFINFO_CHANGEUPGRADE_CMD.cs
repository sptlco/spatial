// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Objects;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_BRIEFINFO_CHANGEUPGRADE_CMD"/>.
/// </summary>
public class PROTO_NC_BRIEFINFO_CHANGEUPGRADE_CMD : ProtocolBuffer
{
    /// <summary>
    /// The related object.
    /// </summary>
    public ushort handle;

    /// <summary>
    /// The related item.
    /// </summary>
    public ushort item;

    /// <summary>
    /// The item's upgrade level.
    /// </summary>
    public byte upgrade;

    /// <summary>
    /// The item's equipment slot.
    /// </summary>
    public byte nSlotNum;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        handle = ReadUInt16();
        item = ReadUInt16();
        upgrade = ReadByte();
        nSlotNum = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(handle);
        Write(item);
        Write(upgrade);
        Write(nSlotNum);
    }
}
