// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> that contains stored item options.
/// </summary>
public class ItemOptionStorage : ProtocolBuffer
{
    /// <summary>
    /// The item's fixed data.
    /// </summary>
    public FixedInfo optioninfo;

    /// <summary>
    /// The item's options.
    /// </summary>
    public Element[] optionlist;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        optioninfo = Read<FixedInfo>();
        optionlist = Read<Element>(8);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(optioninfo);
        Write(optionlist);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        optioninfo.Dispose();

        foreach (var option in optionlist)
        {
            option.Dispose();
        }

        base.Dispose();
    }

    /// <summary>
    /// Fixed item option data.
    /// </summary>
    public class FixedInfo : ProtocolBuffer
    {
        /// <summary>
        /// The item's option count.
        /// </summary>
        public ItemOptionNumber optionnumber;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            optionnumber = Read<ItemOptionNumber>();
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            Write(optionnumber);
        }

        /// <summary>
        /// Dispose of the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Dispose()
        {
            optionnumber.Dispose();

            base.Dispose();
        }
    }

    /// <summary>
    /// An item option.
    /// </summary>
    public class Element : ProtocolBuffer
    {
        /// <summary>
        /// A null <see cref="Element"/>.
        /// </summary>
        public static readonly Element Null = new();

        /// <summary>
        /// The option's type.
        /// </summary>
        public byte itemoption_type = byte.MaxValue;

        /// <summary>
        /// The option's value.
        /// </summary>
        public ushort itemoption_value = ushort.MaxValue;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            itemoption_type = ReadByte();
            itemoption_value = ReadUInt16();
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            Write(itemoption_type);
            Write(itemoption_value);
        }
    }
}