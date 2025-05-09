// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Combat;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_BAT_TARGETTING_REQ"/>.
/// </summary>
public class PROTO_NC_BAT_TARGET_REQ : ProtocolBuffer
{
    /// <summary>
    /// The targeted object.
    /// </summary>
    public ushort target;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        target = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(target);
    }
}
