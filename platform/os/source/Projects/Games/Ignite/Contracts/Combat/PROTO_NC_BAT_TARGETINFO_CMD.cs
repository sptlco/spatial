// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Combat;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_BAT_TARGETINFO_CMD"/>.
/// </summary>
public class PROTO_NC_BAT_TARGETINFO_CMD : ProtocolBuffer
{
    /// <summary>
    /// The target's order.
    /// </summary>
    public byte order;

    /// <summary>
    /// The target's map handle.
    /// </summary>
    public ushort targethandle;

    /// <summary>
    /// The target's health.
    /// </summary>
    public uint targethp;

    /// <summary>
    /// The target's maximum health.
    /// </summary>
    public uint targetmaxhp;

    /// <summary>
    /// The target's spirit.
    /// </summary>
    public uint targetsp;

    /// <summary>
    /// The target's maximum spirit.
    /// </summary>
    public uint targetmaxsp;

    /// <summary>
    /// The target's light.
    /// </summary>
    public uint targetlp;

    /// <summary>
    /// The target's maximum light.
    /// </summary>
    public uint targetmaxlp;

    /// <summary>
    /// The target's level.
    /// </summary>
    public byte targetlevel;

    /// <summary>
    /// The target's health change order.
    /// </summary>
    public ushort hpchangeorder;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        order = ReadByte();
        targethandle = ReadUInt16();
        targethp = ReadUInt32();
        targetmaxhp = ReadUInt32();
        targetsp = ReadUInt32();
        targetmaxsp = ReadUInt32();
        targetlp = ReadUInt32();
        targetmaxlp = ReadUInt32();
        targetlevel = ReadByte();
        hpchangeorder = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(order);
        Write(targethandle);
        Write(targethp);
        Write(targetmaxhp);
        Write(targetsp);
        Write(targetmaxsp);
        Write(targetlp);
        Write(targetmaxlp);
        Write(targetlevel);
        Write(hpchangeorder);
    }
}
