// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing item attributes.
/// </summary>
public class ShineItemAttr_Amulet : SHINE_ITEM_ATTRIBUTE
{
    /// <summary>
    /// The item's expiration date.
    /// </summary>
    public ShineDateTime deletetime;

    /// <summary>
    /// Whether or not the item is bound to its owner.
    /// </summary>
    public SHINE_PUT_ON_BELONGED_ITEM IsPutOnBelonged;

    /// <summary>
    /// The item's upgrade level.
    /// </summary>
    public byte upgrade;

    /// <summary>
    /// The item's strength enhancements.
    /// </summary>
    public byte strengthen;

    /// <summary>
    /// The number of failed item upgrades.
    /// </summary>
    public byte upgradefailcount;

    /// <summary>
    /// The item's upgraded options.
    /// </summary>
    public ItemOptionStorage UpgradeOption;

    /// <summary>
    /// The number of times the item's random options have been changed.
    /// </summary>
    public byte randomOptionChangedCount;

    /// <summary>
    /// The item's options.
    /// </summary>
    public ItemOptionStorage option;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        deletetime = Read<ShineDateTime>();
        IsPutOnBelonged = (SHINE_PUT_ON_BELONGED_ITEM)ReadInt32();
        upgrade = ReadByte();
        strengthen = ReadByte();
        upgradefailcount = ReadByte();
        UpgradeOption = Read<ItemOptionStorage>();
        randomOptionChangedCount = ReadByte();
        option = Read<ItemOptionStorage>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(deletetime);
        Write((int)IsPutOnBelonged);
        Write(upgrade);
        Write(strengthen);
        Write(upgradefailcount);
        Write(UpgradeOption);
        Write(randomOptionChangedCount);
        Write(option);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        deletetime.Dispose();
        UpgradeOption.Dispose();
        option.Dispose();

        base.Dispose();
    }
}