// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing item attributes.
/// </summary>
public class ShineItemAttr_MobCardCollect : SHINE_ITEM_ATTRIBUTE
{
    /// <summary>
    /// The item's serial number.
    /// </summary>
    public uint SerialNumber;

    /// <summary>
    /// The item's star count.
    /// </summary>
    public byte Star;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        SerialNumber = ReadUInt32();
        Star = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(SerialNumber);
        Write(Star);
    }
}
