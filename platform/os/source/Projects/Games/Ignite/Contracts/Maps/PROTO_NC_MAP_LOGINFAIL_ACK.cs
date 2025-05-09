// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Maps;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_MAP_LOGINFAIL_ACK"/>.
/// </summary>
public class PROTO_NC_MAP_LOGINFAIL_ACK : ProtocolBuffer
{
    /// <summary>
    /// A classifying error code.
    /// </summary>
    public ushort err;

    /// <summary>
    /// The index of the file that was manipulated.
    /// </summary>
    public byte nWrongDataFileIndex;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        err = ReadUInt16();
        nWrongDataFileIndex = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(err);
        Write(nWrongDataFileIndex);
    }
}
