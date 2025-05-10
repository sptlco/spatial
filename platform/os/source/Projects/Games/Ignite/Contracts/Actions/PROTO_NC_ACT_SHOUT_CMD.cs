// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Actions;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_ACT_SHOUT_CMD"/>.
/// </summary>
public class PROTO_NC_ACT_SHOUT_CMD : ProtocolBuffer
{
    /// <summary>
    /// The number of linked items.
    /// </summary>
    public byte itemLinkDataCount;

    /// <summary>
    /// The length of the message.
    /// </summary>
    public byte len;

    /// <summary>
    /// The message body.
    /// </summary>
    public byte[] content;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        itemLinkDataCount = ReadByte();
        len = ReadByte();
        content = ReadBytes(len);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(itemLinkDataCount);
        Write(len);
        Write(content, len);
    }
}
