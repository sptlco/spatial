// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_CLIENT_GAME_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_CLIENT_GAME_CMD : ProtocolBuffer
{
    /// <summary>
    /// The character's party identification number.
    /// </summary>
    public ushort partyno;

    /// <summary>
    /// The character's raid identification number.
    /// </summary>
    public ushort nRaidNo;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        partyno = ReadUInt16();
        nRaidNo = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(partyno);
        Write(nRaidNo);
    }
}
