// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Objects;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_BRIEFINFO_BRIEFINFODELETE_CMD"/>.
/// </summary>
public class PROTO_NC_BRIEFINFO_BRIEFINFODELETE_CMD : ProtocolBuffer
{
    /// <summary>
    /// The object's handle.
    /// </summary>
    public ushort hnd;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        hnd = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(hnd);
    }
}
