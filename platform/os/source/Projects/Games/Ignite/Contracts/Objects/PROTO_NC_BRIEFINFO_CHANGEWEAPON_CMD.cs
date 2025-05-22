// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Objects;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_BRIEFINFO_CHANGEWEAPON_CMD"/>.
/// </summary>
public class PROTO_NC_BRIEFINFO_CHANGEWEAPON_CMD : ProtocolBuffer
{
    /// <summary>
    /// The weapon's upgrade information.
    /// </summary>
    public PROTO_NC_BRIEFINFO_CHANGEUPGRADE_CMD upgradeinfo;

    /// <summary>
    /// The weapon's current mob identification number.
    /// </summary>
    public ushort currentmobid;

    /// <summary>
    /// The weapon's current kill level.
    /// </summary>
    public byte currentkilllevel;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        upgradeinfo = Read<PROTO_NC_BRIEFINFO_CHANGEUPGRADE_CMD>();
        currentmobid = ReadUInt16();
        currentkilllevel = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(upgradeinfo);
        Write(currentmobid);
        Write(currentkilllevel);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        upgradeinfo.Dispose();

        base.Dispose();
    }
}
