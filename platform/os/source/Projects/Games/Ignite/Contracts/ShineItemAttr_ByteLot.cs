// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing item attributes.
/// </summary>
public class ShineItemAttr_ByteLot : SHINE_ITEM_ATTRIBUTE
{
    /// <summary>
    /// The item's lot.
    /// </summary>
    public byte lot;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        lot = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(lot);
    }
}
