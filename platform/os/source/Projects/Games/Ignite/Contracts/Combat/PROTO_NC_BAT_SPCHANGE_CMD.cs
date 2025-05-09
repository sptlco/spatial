// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Combat;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_BAT_SPCHANGE_CMD"/>.
/// </summary>
public class PROTO_NC_BAT_SPCHANGE_CMD : ProtocolBuffer
{
    /// <summary>
    /// The object's spirit.
    /// </summary>
    public uint sp;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        sp = ReadUInt32();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(sp);
    }
}