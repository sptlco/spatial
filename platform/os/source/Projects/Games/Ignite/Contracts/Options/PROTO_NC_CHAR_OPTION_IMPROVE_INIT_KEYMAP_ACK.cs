// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Options;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_INIT_KEYMAP_ACK"/>.
/// </summary>
public class PROTO_NC_CHAR_OPTION_IMPROVE_INIT_KEYMAP_ACK : ProtocolBuffer
{
    /// <summary>
    /// A classifying error code.
    /// </summary>
    public ushort nError;

    /// <summary>
    /// Keymap data.
    /// </summary>
    public PROTO_NC_CHAR_OPTION_IMPROVE_GET_KEYMAP_CMD DBKeyMapData;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nError = ReadUInt16();
        DBKeyMapData = Read<PROTO_NC_CHAR_OPTION_IMPROVE_GET_KEYMAP_CMD>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(nError);
        Write(DBKeyMapData);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        DBKeyMapData.Dispose();

        base.Dispose();
    }
}
