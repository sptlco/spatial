// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_CLIENT_SKILL_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_CLIENT_SKILL_CMD : ProtocolBuffer
{
    /// <summary>
    /// The character's remaining skill empower points.
    /// </summary>
    public byte restempow;

    /// <summary>
    /// The <see cref="PARTMARK"/> mark for this <see cref="ProtocolBuffer"/>.
    /// </summary>
    public PARTMARK PartMark;

    /// <summary>
    /// The maximum number of skills.
    /// </summary>
    public ushort nMaxNum;

    /// <summary>
    /// Skill data.
    /// </summary>
    public PROTO_NC_CHAR_SKILLCLIENT_CMD skill;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        restempow = ReadByte();
        PartMark = Read<PARTMARK>();
        nMaxNum = ReadUInt16();
        skill = Read<PROTO_NC_CHAR_SKILLCLIENT_CMD>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(restempow);
        Write(PartMark);
        Write(nMaxNum);
        Write(skill);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        PartMark.Dispose();
        skill.Dispose();

        base.Dispose();
    }
}
