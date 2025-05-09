// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Combat;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_BAT_HPCHANGE_CMD"/>.
/// </summary>
public class PROTO_NC_BAT_HPCHANGE_CMD : ProtocolBuffer
{
    /// <summary>
    /// The object's health.
    /// </summary>
    public uint hp;

    /// <summary>
    /// An ordering for health changes.
    /// </summary>
    public ushort hpchangeorder;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        hp = ReadUInt32();
        hpchangeorder = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(hp);
        Write(hpchangeorder);
    }
}