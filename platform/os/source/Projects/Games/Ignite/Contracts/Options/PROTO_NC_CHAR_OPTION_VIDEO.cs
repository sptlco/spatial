// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Options;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing video data.
/// </summary>
public class PROTO_NC_CHAR_OPTION_VIDEO : ProtocolBuffer
{
    /// <summary>
    /// Video data.
    /// </summary>
    public byte[] Data;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        Data = ReadBytes(60);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(Data);
    }
}
