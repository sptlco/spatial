// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_CHARGEDBUFF_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_CHARGEDBUFF_CMD : ProtocolBuffer
{
    /// <summary>
    /// The charged buff count.
    /// </summary>
    public ushort NumOfChargedBuff;

    /// <summary>
    /// Charged buffs.
    /// </summary>
    public PROTO_CHARGEDBUFF_INFO[] ChargedBuff;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        NumOfChargedBuff = ReadUInt16();
        ChargedBuff = Read<PROTO_CHARGEDBUFF_INFO>(NumOfChargedBuff);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(NumOfChargedBuff);
        Write(ChargedBuff);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var buff in ChargedBuff)
        {
            buff.Dispose();
        }

        base.Dispose();
    }
}
