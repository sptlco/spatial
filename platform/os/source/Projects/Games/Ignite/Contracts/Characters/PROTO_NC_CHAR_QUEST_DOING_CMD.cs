// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_QUEST_DOING_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_QUEST_DOING_CMD : ProtocolBuffer
{
    /// <summary>
    /// The character's identification number.
    /// </summary>
    public uint chrregnum;

    /// <summary>
    /// Undocumented.
    /// </summary>
    public bool bNeedClear;

    /// <summary>
    /// The number of quests in progress.
    /// </summary>
    public byte nNumOfDoingQuest;

    /// <summary>
    /// Quests in progress.
    /// </summary>
    public PLAYER_QUEST_INFO[] QuestDoingArray;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        chrregnum = ReadUInt32();
        bNeedClear = ReadBoolean();
        nNumOfDoingQuest = ReadByte();
        QuestDoingArray = Read<PLAYER_QUEST_INFO>(nNumOfDoingQuest);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(chrregnum);
        Write(bNeedClear);
        Write(nNumOfDoingQuest);
        Write(QuestDoingArray);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var quest in QuestDoingArray)
        {
            quest.Dispose();
        }

        base.Dispose();
    }
}
