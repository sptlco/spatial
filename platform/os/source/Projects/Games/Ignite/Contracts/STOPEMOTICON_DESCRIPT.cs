// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing paused emoticon data.
/// </summary>
public class STOPEMOTICON_DESCRIPT : ProtocolBuffer
{
    /// <summary>
    /// The emoticon's identification number.
    /// </summary>
    public byte emoticonid;

    /// <summary>
    /// The emotion's current frame.
    /// </summary>
    public ushort emoticonframe;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        emoticonid = ReadByte();
        emoticonframe = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(emoticonid);
        Write(emoticonframe);
    }
}
