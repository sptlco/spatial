// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing item attributes.
/// </summary>
public class ShineItemAttr_Boot : SHINE_ITEM_ATTRIBUTE
{
    /// <summary>
    /// The item's upgrade level.
    /// </summary>
    public byte upgrade;

    /// <summary>
    /// The item's strength enhancements.
    /// </summary>
    public byte strengthen;

    /// <summary>
    /// The item's failed upgrade count.
    /// </summary>
    public byte upgradefailcount;

    /// <summary>
    /// The item's binding flags.
    /// </summary>
    public SHINE_PUT_ON_BELONGED_ITEM IsPutOnBelonged;

    /// <summary>
    /// The item's expiration date.
    /// </summary>
    public ShineDateTime deletetime;

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
        upgrade = ReadByte();
        strengthen = ReadByte();
        upgradefailcount = ReadByte();
        IsPutOnBelonged = (SHINE_PUT_ON_BELONGED_ITEM) ReadInt32();
        deletetime = Read<ShineDateTime>();
        randomOptionChangedCount = ReadByte();
        option = Read<ItemOptionStorage>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(upgrade);
        Write(strengthen);
        Write(upgradefailcount);
        Write((int) IsPutOnBelonged);
        Write(deletetime);
        Write(randomOptionChangedCount);
        Write(option);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        deletetime.Dispose();
        option.Dispose();

        base.Dispose();
    }
}
