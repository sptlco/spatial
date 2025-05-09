// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_QUEST_DONE_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_QUEST_DONE_CMD : ProtocolBuffer
{
    /// <summary>
    /// The character's identification number.
    /// </summary>
    public uint chrregnum;

    /// <summary>
    /// The number of completed quests.
    /// </summary>
    public ushort nTotalDoneQuest;

    /// <summary>
    /// The number of completed quests in this <see cref="ProtocolBuffer"/>.
    /// </summary>
    public ushort nTotalDoneQuestSize;

    /// <summary>
    /// The number of completed quests in this <see cref="ProtocolBuffer"/>.
    /// </summary>
    public ushort nDoneQuestCount;

    /// <summary>
    /// The index of this <see cref="ProtocolBuffer"/>.
    /// </summary>
    public ushort nIndex;

    /// <summary>
    /// Completed quests.
    /// </summary>
    public PLAYER_QUEST_DONE_INFO[] QuestDoneArray;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        chrregnum = ReadUInt32();
        nTotalDoneQuest = ReadUInt16();
        nTotalDoneQuestSize = ReadUInt16();
        nDoneQuestCount = ReadUInt16();
        nIndex = ReadUInt16();
        QuestDoneArray = Read<PLAYER_QUEST_DONE_INFO>(nDoneQuestCount);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(chrregnum);
        Write(nTotalDoneQuest);
        Write(nTotalDoneQuestSize);
        Write(nDoneQuestCount);
        Write(nIndex);
        Write(QuestDoneArray);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var quest in QuestDoneArray)
        {
            quest.Dispose();
        }

        base.Dispose();
    }
}
