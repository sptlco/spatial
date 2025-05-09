// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Actions;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_ACT_STOP_REQ"/>.
/// </summary>
public class PROTO_NC_ACT_STOP_REQ : ProtocolBuffer
{
    /// <summary>
    /// The object's position.
    /// </summary>
    public SHINE_XY_TYPE loc;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        loc = Read<SHINE_XY_TYPE>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
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
