// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing item attributes.
/// </summary>
public class ShineItemAttr_MobCardCollect_Unident : SHINE_ITEM_ATTRIBUTE
{
    /// <summary>
    /// The item's serial number.
    /// </summary>
    public uint SerialNumber;

    /// <summary>
    /// The item's card identification number.
    /// </summary>
    public ushort CardID;

    /// <summary>
    /// The item's star count.
    /// </summary>
    public byte Star;

    /// <summary>
    /// The item's card group identification number.
    /// </summary>
    public ushort Group;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        SerialNumber = ReadUInt32();
        CardID = ReadUInt16();
        Star = ReadByte();
        Group = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(SerialNumber);
        Write(CardID);
        Write(Star);
        Write(Group);
    }
}
