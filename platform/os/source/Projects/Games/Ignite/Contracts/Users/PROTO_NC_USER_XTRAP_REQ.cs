// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Users;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_USER_XTRAP_REQ"/>.
/// </summary>
public class PROTO_NC_USER_XTRAP_REQ : ProtocolBuffer
{
    /// <summary>
    /// The length of the client's XTrap key.
    /// </summary>
    public byte XTrapClientKeyLength;

    /// <summary>
    /// The client's XTrap key.
    /// </summary>
    public byte[] XTrapClientKey;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        XTrapClientKeyLength = ReadByte();
        XTrapClientKey = ReadBytes(XTrapClientKeyLength);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(XTrapClientKeyLength);
        Write(XTrapClientKey, XTrapClientKeyLength);
    }
}
