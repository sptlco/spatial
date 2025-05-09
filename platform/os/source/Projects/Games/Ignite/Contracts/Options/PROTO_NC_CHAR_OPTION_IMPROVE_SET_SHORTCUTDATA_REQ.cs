// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Options;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_SET_SHORTCUTDATA_REQ"/>.
/// </summary>
public class PROTO_NC_CHAR_OPTION_IMPROVE_SET_SHORTCUTDATA_REQ : ProtocolBuffer
{
    /// <summary>
    /// The number of shortcuts.
    /// </summary>
    public byte nShortCutDataCnt;

    /// <summary>
    /// Shortcut data.
    /// </summary>
    public SHORT_CUT_DATA[] ShortCutData;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nShortCutDataCnt = ReadByte();
        ShortCutData = Read<SHORT_CUT_DATA>(nShortCutDataCnt);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(nShortCutDataCnt);
        Write(ShortCutData);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var option in ShortCutData)
        {
            option.Dispose();
        }

        base.Dispose();
    }
}
