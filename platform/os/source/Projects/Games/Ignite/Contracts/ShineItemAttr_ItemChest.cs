// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing item attributes.
/// </summary>
public class ShineItemAttr_ItemChest : SHINE_ITEM_ATTRIBUTE
{
    /// <summary>
    /// The item's type.
    /// </summary>
    public Type type;

    /// <summary>
    /// The contents of the chest.
    /// </summary>
    public SHINE_ITEM_REGISTNUMBER[] content;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        type = Read<Type>();
        content = Read<SHINE_ITEM_REGISTNUMBER>(8);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(type);
        Write(content);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        type.Dispose();

        foreach (var item in content)
        {
            item.Dispose();
        }

        base.Dispose();
    }

    /// <summary>
    /// A <see cref="ProtocolBuffer"/> containing item chest data.
    /// </summary>
    public class Type : ProtocolBuffer
    {
        /// <summary>
        /// The number of items in the chest.
        /// </summary>
        public byte itemnum;

        /// <summary>
        /// The item's flags.
        /// </summary>
        public byte flag;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            var type = ReadByte();

            itemnum = (byte) (type & 0xF);
            flag = (byte) ((type >> 4) & 0xF);
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            Write(((flag & 0xF) << 4) | (itemnum & 0xF));
        }
    }
}
