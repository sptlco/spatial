// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Options;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing game data.
/// </summary>
public class PROTO_NC_CHAR_OPTION_GAME : ProtocolBuffer
{
    /// <summary>
    /// Game data.
    /// </summary>
    public byte[] Data;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        Data = ReadBytes(64);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(Data);
    }
}
