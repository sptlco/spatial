// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Users;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_USER_XTRAP_ACK"/>.
/// </summary>
public class PROTO_NC_USER_XTRAP_ACK : ProtocolBuffer
{
    /// <summary>
    /// Whether or not the check was successful.
    /// </summary>
    public bool bSuccess;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        bSuccess = ReadBoolean();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(bSuccess);
    }
}
