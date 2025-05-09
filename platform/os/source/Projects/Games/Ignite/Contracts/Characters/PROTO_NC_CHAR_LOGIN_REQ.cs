// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_LOGIN_REQ"/>.
/// </summary>
public class PROTO_NC_CHAR_LOGIN_REQ : ProtocolBuffer
{
    /// <summary>
    /// The character's slot number.
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
