// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing completed quest data.
/// </summary>
public class PLAYER_QUEST_DONE_INFO : ProtocolBuffer
{
    /// <summary>
    /// The quest's identification number.
    /// </summary>
    public ushort ID;

    /// <summary>
    /// The time the quest was completed.
    /// </summary>
    public long tEndTime;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        ID = ReadUInt16();
        tEndTime = ReadInt64();
    }

    public override void Serialize()
    {
        Write(ID);
        Write(tEndTime);
    }
}
