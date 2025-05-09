// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_CLIENT_SKILL_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_SKILLCLIENT_CMD : ProtocolBuffer
{
    /// <summary>
    /// The character's identification number.
    /// </summary>
    public uint chrregnum;

    /// <summary>
    /// The character's skill count.
    /// </summary>
    public ushort number;

    /// <summary>
    /// Skill data.
    /// </summary>
    public PROTO_SKILLREADBLOCKCLIENT[] skill;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        chrregnum = ReadUInt32();
        number = ReadUInt16();
        skill = Read<PROTO_SKILLREADBLOCKCLIENT>(number);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(chrregnum);
        Write(number);
        Write(skill);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var buffer in skill)
        {
            buffer.Dispose();
        }

        base.Dispose();
    }
}
