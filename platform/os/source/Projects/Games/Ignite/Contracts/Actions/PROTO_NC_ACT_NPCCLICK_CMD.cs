// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Actions;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_ACT_NPCCLICK_CMD"/>.
/// </summary>
public class PROTO_NC_ACT_NPCCLICK_CMD : ProtocolBuffer
{
    /// <summary>
    /// The NPC's object handle.
    /// </summary>
    public ushort npchandle;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        npchandle = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(npchandle);
    }
}
