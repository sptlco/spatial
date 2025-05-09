// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_REVIVESAME_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_REVIVESAME_CMD : ProtocolBuffer
{
    /// <summary>
    /// The map's identification number.
    /// </summary>
    public ushort mapid;

    /// <summary>
    /// The player's location.
    /// </summary>
    public SHINE_XY_TYPE location;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        mapid = ReadUInt16();
        location = Read<SHINE_XY_TYPE>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(mapid);
        Write(location);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        location.Dispose();

        base.Dispose();
    }
}
