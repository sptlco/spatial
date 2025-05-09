// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Combat;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_BAT_LPCHANGE_CMD"/>.
/// </summary>
public class PROTO_NC_BAT_LPCHANGE_CMD : ProtocolBuffer
{
    /// <summary>
    /// The object's light.
    /// </summary>
    public uint nLP;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nLP = ReadUInt32();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(nLP);
    }
}