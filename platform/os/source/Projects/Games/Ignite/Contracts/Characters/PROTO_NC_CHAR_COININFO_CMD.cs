// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_COININFO_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_COININFO_CMD : ProtocolBuffer
{
    /// <summary>
    /// The character's coin balance.
    /// </summary>
    public ulong nCoin;

    /// <summary>
    /// The character's exchanged coin balance.
    /// </summary>
    public ulong nExchangedCoin;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nCoin = ReadUInt64();
        nExchangedCoin = ReadUInt64();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(nCoin);
        Write(nExchangedCoin);
    }
}
