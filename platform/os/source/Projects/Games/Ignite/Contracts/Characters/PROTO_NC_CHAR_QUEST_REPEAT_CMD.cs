// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_QUEST_REPEAT_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_QUEST_REPEAT_CMD : ProtocolBuffer
{
    /// <summary>
    /// The character's identification number.
    /// </summary>
    public uint chrregnum;

    /// <summary>
    /// The number of repeatable quests.
    /// </summary>
    public ushort nNumOfRepeatQuest;

    /// <summary>
    /// Repeatable quests.
    /// </summary>
    public PLAYER_QUEST_INFO[] QuestRepeatArray;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        chrregnum = ReadUInt32();
        nNumOfRepeatQuest = ReadUInt16();
        QuestRepeatArray = Read<PLAYER_QUEST_INFO>(nNumOfRepeatQuest);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(chrregnum);
        Write(nNumOfRepeatQuest);
        Write(QuestRepeatArray);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var quest in QuestRepeatArray)
        {
            quest.Dispose();
        }

        base.Dispose();
    }
}
