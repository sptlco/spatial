// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Objects;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_BRIEFINFO_MOB_CMD"/>.
/// </summary>
public class PROTO_NC_BRIEFINFO_MOB_CMD : ProtocolBuffer
{
    /// <summary>
    /// The number of mobs.
    /// </summary>
    public byte mobnum;

    /// <summary>
    /// A list of mobs.
    /// </summary>
    public PROTO_NC_BRIEFINFO_REGENMOB_CMD[] mobs;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        mobnum = ReadByte();
        mobs = Read<PROTO_NC_BRIEFINFO_REGENMOB_CMD>(mobnum);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(mobnum);
        Write(mobs);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var mob in mobs)
        {
            mob.Dispose();
        }

        base.Dispose();
    }
}