// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_QUEST_READ_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_QUEST_READ_CMD : ProtocolBuffer
{
    /// <summary>
    /// The character's identification number.
    /// </summary>
    public uint chrregnum;

    /// <summary>
    /// The number of quests read.
    /// </summary>
    public ushort nNumOfReadQuest;

    /// <summary>
    /// Quests read.
    /// </summary>
    public ushort[] QuestReadIDArray;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        chrregnum = ReadUInt32();
        nNumOfReadQuest = ReadUInt16();
        QuestReadIDArray = Read<ushort>(nNumOfReadQuest);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(chrregnum);
        Write(nNumOfReadQuest);
        Write(QuestReadIDArray);
    }
}
