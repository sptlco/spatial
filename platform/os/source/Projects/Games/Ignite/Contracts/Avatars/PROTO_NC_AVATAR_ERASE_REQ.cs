// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Avatars;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_AVATAR_ERASE_REQ"/>.
/// /// </summary>
public class PROTO_NC_AVATAR_ERASE_REQ : ProtocolBuffer
{
    /// <summary>
    /// The avatar to erase.
    /// </summary>
    public byte slot;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        slot = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(slot);
    }
}
