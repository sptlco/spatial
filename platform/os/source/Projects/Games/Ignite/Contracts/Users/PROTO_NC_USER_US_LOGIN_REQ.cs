// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Users;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_USER_US_LOGIN_REQ"/>.
/// </summary>
public class PROTO_NC_USER_US_LOGIN_REQ : ProtocolBuffer
{
    /// <summary>
    /// The user's username.
    /// </summary>
    public string sUserName;

    /// <summary>
    /// The user's password.
    /// </summary>
    public string sPassword;

    /// <summary>
    /// A deprecated property that should no longer be used.
    /// </summary>
    public string spawnapps;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        sUserName = ReadString(256);
        sPassword = ReadString(36);
        spawnapps = ReadString(20);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(sUserName, 256);
        Write(sPassword, 36);
        Write(spawnapps, 20);
    }
}
