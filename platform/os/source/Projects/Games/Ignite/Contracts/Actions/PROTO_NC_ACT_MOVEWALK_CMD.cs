// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Actions;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_ACT_MOVEWALK_CMD"/>
/// </summary>
public class PROTO_NC_ACT_MOVEWALK_CMD : ProtocolBuffer
{
    /// <summary>
    /// The object's starting position.
    /// </summary>
    public SHINE_XY_TYPE from;

    /// <summary>
    /// The object's destination.
    /// </summary>
    public SHINE_XY_TYPE to;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        from = Read<SHINE_XY_TYPE>();
        to = Read<SHINE_XY_TYPE>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(from);
        Write(to);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        from.Dispose();
        to.Dispose();

        base.Dispose();
    }
}
