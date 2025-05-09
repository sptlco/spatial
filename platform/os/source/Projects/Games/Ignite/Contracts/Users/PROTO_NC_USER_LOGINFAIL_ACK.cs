// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Users;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_USER_LOGINFAIL_ACK"/>.
/// </summary>
public class PROTO_NC_USER_LOGINFAIL_ACK : ProtocolBuffer
{
    /// <summary>
    /// The error that occurred.
    /// </summary>
    public ushort error;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        error = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(error);
    }
}
