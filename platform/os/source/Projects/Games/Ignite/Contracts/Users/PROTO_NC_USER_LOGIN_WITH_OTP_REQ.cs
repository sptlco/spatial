// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Users;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_USER_LOGIN_WITH_OTP_REQ"/>.
/// </summary>
public class PROTO_NC_USER_LOGIN_WITH_OTP_REQ : ProtocolBuffer
{
    /// <summary>
    /// A one-time password.
    /// </summary>
    public string sOTP;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        sOTP = ReadString(32);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(sOTP, 32);
    }
}
