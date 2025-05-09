// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Options;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing shortcut data.
/// </summary>
public class PROTO_NC_CHAR_OPTION_SHORTCUTDATA : ProtocolBuffer
{
    /// <summary>
    /// Shortcut data.
    /// </summary>
    public byte[] Data;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        Data = ReadBytes(1024);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(Data);
    }
}
