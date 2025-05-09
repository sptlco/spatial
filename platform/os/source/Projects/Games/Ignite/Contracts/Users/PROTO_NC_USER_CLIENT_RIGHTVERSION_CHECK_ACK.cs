// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Users;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_USER_CLIENT_RIGHTVERSION_CHECK_ACK"/>.
/// </summary>
public class PROTO_NC_USER_CLIENT_RIGHTVERSION_CHECK_ACK : ProtocolBuffer
{
    /// <summary>
    /// The length of the server's XTrap key.
    /// </summary>
    public byte XTrapServerKeyLength;

    /// <summary>
    /// The server's XTrap key.
    /// </summary>
    public byte[] XTrapServerKey;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        XTrapServerKeyLength = ReadByte();
        XTrapServerKey = ReadBytes(XTrapServerKeyLength);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(XTrapServerKeyLength);
        Write(XTrapServerKey, XTrapServerKeyLength);
    }
}
