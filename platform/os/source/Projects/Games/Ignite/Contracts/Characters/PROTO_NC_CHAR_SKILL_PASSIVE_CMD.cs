// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_SKILL_PASSIVE_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_SKILL_PASSIVE_CMD : ProtocolBuffer
{
    /// <summary>
    /// The number of passive skills.
    /// </summary>
    public ushort number;

    /// <summary>
    /// Passive skills.
    /// </summary>
    public ushort[] passive;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        number = ReadUInt16();
        passive = Read<ushort>(number);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(number);
        Write(passive);
    }
}
