// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Avatars;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_AVATAR_CREATE_REQ"/>.
/// </summary>
public sealed class PROTO_NC_AVATAR_CREATE_REQ : ProtocolBuffer
{
    /// <summary>
    /// The avatar's slot.
    /// </summary>
    public byte slotnum;

    /// <summary>
    /// The avatar's name.
    /// </summary>
    public string name;

    /// <summary>
    /// The avatar's shape.
    /// </summary>
    public PROTO_AVATAR_SHAPE_INFO char_shape;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        slotnum = ReadByte();
        name = ReadString(20);
        char_shape = Read<PROTO_AVATAR_SHAPE_INFO>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(slotnum);
        Write(name, 20);
        Write(char_shape);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        char_shape.Dispose();

        base.Dispose();
    }
}
