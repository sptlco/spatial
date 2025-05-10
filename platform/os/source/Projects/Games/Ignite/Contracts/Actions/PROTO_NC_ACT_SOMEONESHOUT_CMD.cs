// Copyright © Spatial. All rights reserved.

using System;
using System.Text;
using Spatial.Networking;

namespace Ignite.Contracts.Actions;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_ACT_SOMEONESHOUT_CMD"/>.
/// </summary>
public class PROTO_NC_ACT_SOMEONESHOUT_CMD : ProtocolBuffer
{
    /// <summary>
    /// The number of linked items.
    /// </summary>
    public byte itemLinkDataCount;

    /// <summary>
    /// The message's speaker.
    /// </summary>
    public Speaker speaker;

    /// <summary>
    /// The message's flags.
    /// </summary>
    public Flags flag;

    /// <summary>
    /// The length of the message.
    /// </summary>
    public byte len;

    /// <summary>
    /// The message's body.
    /// </summary>
    public byte[] content;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        itemLinkDataCount = ReadByte();
        speaker = Read<Speaker>();
        flag = Read<Flags>();
        len = ReadByte();
        content = ReadBytes(len);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(itemLinkDataCount);
        Write(speaker);
        Write(flag);
        Write(len);
        Write(content, len);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        speaker.Dispose();
        flag.Dispose();

        base.Dispose();
    }

    /// <summary>
    /// The speaker of a shout message.
    /// </summary>
    public class Speaker : ProtocolBuffer
    {
        /// <summary>
        /// A character's name.
        /// </summary>
        public string charID = string.Empty;

        /// <summary>
        /// A mob identification number.
        /// </summary>
        public ushort mobID = ushort.MaxValue;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            var bytes = ReadBytes(20);

            charID = Encoding.ASCII.GetString(bytes);
            mobID = BitConverter.ToUInt16(bytes);
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            if (!string.IsNullOrEmpty(charID))
            {
                Write(charID, 20);
            }
            else if (mobID != ushort.MaxValue)
            {
                Write(mobID);
                Fill(18, 0);
            }
            else
            {
                throw new ArgumentException("No speaker defined.");
            }
        }
    }

    /// <summary>
    /// The message's flags.
    /// </summary>
    public class Flags : ProtocolBuffer
    {
        /// <summary>
        /// Whether or not to color the message using GM color styles.
        /// </summary>
        public bool GMColor;

        /// <summary>
        /// Whether or not the message is from a mob.
        /// </summary>
        public bool isMob;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            var flags = ReadByte();

            GMColor = (flags & 0x1) != 0;
            isMob = ((flags >> 1) & 0x1) != 0;
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            Write((byte) ((((byte) (isMob ? 1 : 0) & 0x1) << 1) | (byte) (GMColor ? 1 : 0) & 0x1));
        }
    }
}
