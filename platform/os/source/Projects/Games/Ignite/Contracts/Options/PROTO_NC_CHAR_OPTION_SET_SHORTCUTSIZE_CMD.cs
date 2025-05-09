// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Options;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_OPTION_SET_SHORTCUTSIZE_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_OPTION_SET_SHORTCUTSIZE_CMD : ProtocolBuffer
{
    /// <summary>
    /// Shortcut size data.
    /// </summary>
    public PROTO_NC_CHAR_OPTION_SHORTCUTSIZE Data;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        Data = Read<PROTO_NC_CHAR_OPTION_SHORTCUTSIZE>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(Data);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        Data.Dispose();

        base.Dispose();
    }
}
