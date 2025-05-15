// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing character data.
/// </summary>
public class CHARBRIEFINFO_NOTCAMP : ProtocolBuffer
{
    /// <summary>
    /// The character's equipment.
    /// </summary>
    public PROTO_EQUIPMENT equipment;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        equipment = Read<PROTO_EQUIPMENT>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(equipment);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        equipment.Dispose();

        base.Dispose();
    }
}
