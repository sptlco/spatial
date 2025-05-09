// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Options;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_GET_GAMEOPTION_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_OPTION_IMPROVE_GET_GAMEOPTION_CMD : ProtocolBuffer
{
    /// <summary>
    /// The number of game options.
    /// </summary>
    public ushort nGameOptionDataCnt;

    /// <summary>
    /// Game option data.
    /// </summary>
    public GAME_OPTION_DATA[] GameOptionData;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nGameOptionDataCnt = ReadUInt16();
        GameOptionData = Read<GAME_OPTION_DATA>(nGameOptionDataCnt);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(nGameOptionDataCnt);
        Write(GameOptionData);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var option in GameOptionData)
        {
            option.Dispose();
        }

        base.Dispose();
    }
}
