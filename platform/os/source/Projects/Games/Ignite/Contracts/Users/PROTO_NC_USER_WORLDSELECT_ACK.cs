// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Users;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_USER_WORLDSELECT_ACK"/>.
/// </summary>
public class PROTO_NC_USER_WORLDSELECT_ACK : ProtocolBuffer
{
    /// <summary>
    /// The selected world's current status.
    /// </summary>
    public byte worldstatus;

    /// <summary>
    /// The selected world's IP address.
    /// </summary>
    public string ip;

    /// <summary>
    /// The selected world's port number.
    /// </summary>
    public ushort port;

    /// <summary>
    /// A validation key.
    /// </summary>
    public ushort[] validate_new;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        worldstatus = ReadByte();
        ip = ReadString(16);
        port = ReadUInt16();
        validate_new = Read<ushort>(32);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(worldstatus);
        Write(ip, 16);
        Write(port);
        Write(validate_new);
    }
}
