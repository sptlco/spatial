// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Actions;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_ACT_SOMEONEMOVEWALK_CMD"/>.
/// </summary>
public class PROTO_NC_ACT_SOMEONEMOVEWALK_CMD : ProtocolBuffer
{
    /// <summary>
    /// The moving object's handle.
    /// </summary>
    public ushort handle;

    /// <summary>
    /// The object's starting position.
    /// </summary>
    public SHINE_XY_TYPE from;

    /// <summary>
    /// The object's destination.
    /// </summary>
    public SHINE_XY_TYPE to;

    /// <summary>
    /// The object's speed.
    /// </summary>
    public ushort speed;

    /// <summary>
    /// The object's movement attributes.
    /// </summary>
    public Attributes moveattr;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        handle = ReadUInt16();
        from = Read<SHINE_XY_TYPE>();
        to = Read<SHINE_XY_TYPE>();
        speed = ReadUInt16();
        moveattr = Read<Attributes>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(handle);
        Write(from);
        Write(to);
        Write(speed);
        Write(moveattr);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        from.Dispose();
        to.Dispose();
        moveattr.Dispose();

        base.Dispose();
    }

    /// <summary>
    /// Movement attributes.
    /// </summary>
    public class Attributes : ProtocolBuffer
    {
        /// <summary>
        /// Undocumented.
        /// </summary>
        public bool direct;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            direct = (ReadUInt16() & 0x1) != 0;
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            Write((ushort) ((direct ? 1 : 0) & 0x1));
        }
    }
}