// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_ADMIN_LEVEL_INFORM_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_ADMIN_LEVEL_INFORM_CMD : ProtocolBuffer
{
    /// <summary>
    /// The character's administrator level.
    /// </summary>
    public byte nAdminLevel;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nAdminLevel = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Serialize()
    {
        Write(nAdminLevel);
    }
}
