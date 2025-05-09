// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Users;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_USER_LOGINWORLD_ACK"/>.
/// </summary>
public sealed class PROTO_NC_USER_LOGINWORLD_ACK : ProtocolBuffer
{
    /// <summary>
    /// The user's world manager handle.
    /// </summary>
    public ushort worldmanager;

    /// <summary>
    /// The user's avatar count.
    /// </summary>
    public byte numofavatar;

    /// <summary>
    /// The user's avatars.
    /// </summary>
    public PROTO_AVATARINFORMATION[] avatar;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        worldmanager = ReadUInt16();
        numofavatar = ReadByte();
        avatar = Read<PROTO_AVATARINFORMATION>(numofavatar);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(worldmanager);
        Write(numofavatar);
        Write(avatar);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var character in avatar)
        {
            character.Dispose();
        }

        base.Dispose();
    }
}
