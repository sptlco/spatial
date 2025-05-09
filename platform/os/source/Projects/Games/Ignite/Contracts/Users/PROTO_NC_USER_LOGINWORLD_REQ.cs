// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Users;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_USER_LOGINWORLD_REQ"/>.
/// </summary>
public class PROTO_NC_USER_LOGINWORLD_REQ : ProtocolBuffer
{
    /// <summary>
    /// The name of the user.
    /// </summary>
    public string user;

    /// <summary>
    /// The user's validation key.
    /// </summary>
    public ushort[] validate_new;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        user = ReadString(256);
        validate_new = Read<ushort>(32);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(user, 256);
        Write(validate_new);
    }
}
