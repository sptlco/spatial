// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Prison;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_PRISON_GET_ACK"/>.
/// </summary>
public class PROTO_NC_PRISON_GET_ACK : ProtocolBuffer
{
    /// <summary>
    /// A classifying error code.
    /// </summary>
    public ushort err;

    /// <summary>
    /// The number of minutes the character is imprisoned for.
    /// </summary>
    public ushort nMinute;

    /// <summary>
    /// The reason the character is imprisoned.
    /// </summary>
    public string sReason;
    
    /// <summary>
    /// Remarks concerning the imprisonment.
    /// </summary>
    public string sRemark;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        err = ReadUInt16();
        nMinute = ReadUInt16();
        sReason = ReadString(16);
        sRemark = ReadString(64);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(err);
        Write(nMinute);
        Write(sReason, 16);
        Write(sRemark, 64);
    }
}
