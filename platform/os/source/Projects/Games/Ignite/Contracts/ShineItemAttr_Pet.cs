// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing item attributes.
/// </summary>
public class ShineItemAttr_Pet : SHINE_ITEM_ATTRIBUTE
{
    /// <summary>
    /// The pet's registration number.
    /// </summary>
    public uint nPetRegNum;

    /// <summary>
    /// The pet's identification number.
    /// </summary>
    public uint nPetID;

    /// <summary>
    /// The pet's name.
    /// </summary>
    public string sName;

    /// <summary>
    /// Whether or not the pet is being summoned.
    /// </summary>
    public bool bSummoning;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nPetRegNum = ReadUInt32();
        nPetID = ReadUInt32();
        sName = ReadString(17);
        bSummoning = ReadByte() != 0;
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(nPetRegNum);
        Write(nPetID);
        Write(sName, 17);
        Write((byte) (bSummoning ? 1 : 0));
    }
}
