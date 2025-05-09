// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Users;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_USER_NORMALLOGOUT_CMD"/>.
/// </summary>
public class PROTO_NC_USER_NORMALLOGOUT_CMD : ProtocolBuffer
{
    /// <summary>
    /// The type of logout that was performed.
    /// </summary>
    public byte LogoutType;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        LogoutType = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(LogoutType);
    }
}
