// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Users;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_USER_WORLDSELECT_REQ"/>.
/// </summary>
public class PROTO_NC_USER_WORLDSELECT_REQ : ProtocolBuffer
{
    /// <summary>
    /// The selected world's identification number.
    /// </summary>
    public byte worldno;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        worldno = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(worldno);
    }
}
