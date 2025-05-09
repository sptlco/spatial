// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Avatars;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_AVATAR_CREATESUCC_ACK"/>.
/// </summary>
public sealed class PROTO_NC_AVATAR_CREATESUCC_ACK : ProtocolBuffer
{
    /// <summary>
    /// The account's current avatar count.
    /// </summary>
    public byte numofavatar;

    /// <summary>
    /// The avatar that was created.
    /// </summary>
    public PROTO_AVATARINFORMATION avatar;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        numofavatar = ReadByte();
        avatar = Read<PROTO_AVATARINFORMATION>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(numofavatar);
        Write(avatar);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        avatar.Dispose();

        base.Dispose();
    }
}
