// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Actions;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_ACT_SOMEONECHAT_CMD"/>.
/// </summary>
public class PROTO_NC_ACT_SOMEONECHAT_CMD : ProtocolBuffer
{
    /// <summary>
    /// The number of items linked to the message.
    /// </summary>
    public byte itemLinkDataCount;

    /// <summary>
    /// The object that sent the message.
    /// </summary>
    public ushort handle;

    /// <summary>
    /// The length of the message.
    /// </summary>
    public byte len;

    /// <summary>
    /// Flags for the message.
    /// </summary>
    public Flags flag;

    /// <summary>
    /// The message's font color.
    /// </summary>
    public byte nChatFontColorID;

    /// <summary>
    /// The color of the message's balloon.
    /// </summary>
    public byte nChatBalloonColorID;

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
        handle = ReadUInt16();
        len = ReadByte();
        flag = Read<Flags>();
        nChatFontColorID = ReadByte();
        nChatBalloonColorID = ReadByte();
        content = ReadBytes(len);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(itemLinkDataCount);
        Write(handle);
        Write(len);
        Write(flag);
        Write(nChatFontColorID);
        Write(nChatBalloonColorID);
        Write(content);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        flag.Dispose();

        base.Dispose();
    }

    /// <summary>
    /// A <see cref="ProtocolBuffer"/> containing message flags.
    /// </summary>
    public class Flags : ProtocolBuffer
    {
        /// <summary>
        /// Whether or not the message is GM-colored.
        /// </summary>
        public bool GMColor;

        /// <summary>
        /// Whether or not to display the message in the chat window.
        /// </summary>
        public bool chatwin;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            var flags = ReadByte();

            GMColor = (flags & 0x1) != 0;
            chatwin = ((flags >> 1) & 0x1) != 0;
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            Write((byte) ((((byte) (chatwin ? 1 : 0) & 0x1) << 1) | ((byte) (GMColor ? 1 : 0) & 0x1)));
        }
    }
}