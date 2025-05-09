// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Options;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing shortcut size data.
/// </summary>
public class PROTO_NC_CHAR_OPTION_SHORTCUTSIZE : ProtocolBuffer
{
    /// <summary>
    /// Shortcut size data.
    /// </summary>
    public byte[] Data;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        Data = ReadBytes(24);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(Data);
    }
}
