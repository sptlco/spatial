// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Users;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_USER_WILL_WORLD_SELECT_ACK"/>.
/// </summary>
public class PROTO_NC_USER_WILL_WORLD_SELECT_ACK : ProtocolBuffer
{
    /// <summary>
    /// A classifying error code.
    /// </summary>
    public ushort nError;

    /// <summary>
    /// A one-time password.
    /// </summary>
    public string sOTP;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nError = ReadUInt16();
        sOTP = ReadString(32);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(nError);
        Write(sOTP, 32);
    }
}
