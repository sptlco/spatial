// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing item attributes.
/// </summary>
public class ShineItemAttr_DwrdLot : SHINE_ITEM_ATTRIBUTE
{
    /// <summary>
    /// The item's lot.
    /// </summary>
    public uint lot;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        lot = ReadUInt32();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(lot);
    }
}
