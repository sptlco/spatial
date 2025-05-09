// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Options;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_SET_KEYMAP_REQ"/>.
/// </summary>
public class PROTO_NC_CHAR_OPTION_IMPROVE_SET_KEYMAP_REQ : ProtocolBuffer
{
    /// <summary>
    /// The number of keymaps.
    /// </summary>
    public ushort nKeyMapDataCnt;

    /// <summary>
    /// Keymap data.
    /// </summary>
    public KEY_MAP_DATA[] KeyMapData;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nKeyMapDataCnt = ReadUInt16();
        KeyMapData = Read<KEY_MAP_DATA>(nKeyMapDataCnt);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(nKeyMapDataCnt);
        Write(KeyMapData);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var option in KeyMapData)
        {
            option.Dispose();
        }

        base.Dispose();
    }
}
