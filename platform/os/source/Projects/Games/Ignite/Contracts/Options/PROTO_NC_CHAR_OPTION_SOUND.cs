// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Options;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing sound data.
/// </summary>
public class PROTO_NC_CHAR_OPTION_SOUND : ProtocolBuffer
{
    /// <summary>
    /// Sound data.
    /// </summary>
    public byte[] Data;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public override void Deserialize()
    {
        Data = ReadBytes(1);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public override void Serialize()
    {
        Write(Data);
    }
}
