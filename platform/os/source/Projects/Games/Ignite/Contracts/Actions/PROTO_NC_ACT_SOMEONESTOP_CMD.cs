// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Actions;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_ACT_SOMEONESTOP_CMD"/>.
/// </summary>
public class PROTO_NC_ACT_SOMEONESTOP_CMD : ProtocolBuffer
{
    /// <summary>
    /// The object's handle.
    /// </summary>
    public ushort handle;

    /// <summary>
    /// The object's location.
    /// </summary>
    public SHINE_XY_TYPE loc;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        handle = ReadUInt16();
        loc = Read<SHINE_XY_TYPE>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(handle);
        Write(loc);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        loc.Dispose();

        base.Dispose();
    }
}
