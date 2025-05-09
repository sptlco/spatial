// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Users;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_USER_CLIENT_VERSION_CHECK_REQ"/>.
/// </summary>
public class PROTO_NC_USER_CLIENT_VERSION_CHECK_REQ : ProtocolBuffer
{
    /// <summary>
    /// The client's version.
    /// </summary>
    public string sVersionKey;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        sVersionKey = ReadString(64);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(sVersionKey, 64);
    }
}
