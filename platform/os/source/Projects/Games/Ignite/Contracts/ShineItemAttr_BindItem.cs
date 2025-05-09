// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing item attributes.
/// </summary>
public class ShineItemAttr_BindItem : SHINE_ITEM_ATTRIBUTE
{
    /// <summary>
    /// The item's portal count.
    /// </summary>
    public byte portalnum;

    /// <summary>
    /// The item's portals.
    /// </summary>
    public Bind[] portal;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        portalnum = ReadByte();
        portal = Read<Bind>(10);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(portalnum);
        Write(portal);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var bind in portal)
        {
            bind.Dispose();
        }

        base.Dispose();
    }

    /// <summary>
    /// A <see cref="ProtocolBuffer"/> containing bind data.
    /// </summary>
    public class Bind : ProtocolBuffer
    {
        /// <summary>
        /// A null <see cref="Bind"/>.
        /// </summary>
        public static readonly Bind Null = new();

        /// <summary>
        /// The map's identification number.
        /// </summary>
        public ushort mapid;

        /// <summary>
        /// The destination X-coordinate.
        /// </summary>
        public uint x;

        /// <summary>
        /// The destination Y-coordinate.
        /// </summary>
        public uint y;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            mapid = ReadUInt16();
            x = ReadUInt32();
            y = ReadUInt32();
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            Write(mapid);
            Write(x);
            Write(y);
        }
    }
}
