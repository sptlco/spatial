// Copyright © Spatial. All rights reserved.

using System.Collections.Concurrent;
using Ignite.Contracts;

namespace Ignite.Assets.Types;

[Name("ItemInfo.shn")]
public class ItemInfo
{
	private static readonly ConcurrentDictionary<ushort, (ItemInfo Client, ItemInfoServer Server)> _cacheById = [];

	public ushort ID { get; set; }

	public string InxName { get; set; }

	public string Name { get; set; }

	public uint Type { get; set; }

	public ItemClassEnum Class { get; set; }

	public uint MaxLot { get; set; }

	public ItemEquipEnum Equip { get; set; }

	public uint ItemAuctionGroup { get; set; }

	public uint ItemGradeType { get; set; }

	public byte TwoHand { get; set; }

	public uint AtkSpeed { get; set; }

	public uint DemandLv { get; set; }

	public uint Grade { get; set; }

	public uint MinWC { get; set; }

	public uint MaxWC { get; set; }

	public uint AC { get; set; }

	public uint MinMA { get; set; }

	public uint MaxMA { get; set; }

	public uint MR { get; set; }

	public uint TH { get; set; }

	public uint TB { get; set; }

	public uint WCRate { get; set; }

	public uint MARate { get; set; }

	public uint ACRate { get; set; }

	public uint MRRate { get; set; }

	public uint CriRate { get; set; }

	public uint CriMinWc { get; set; }

	public uint CriMaxWc { get; set; }

	public uint CriMinMa { get; set; }

	public uint CriMaxMa { get; set; }

	public uint CrlTB { get; set; }

	public uint UseClass { get; set; }

	public uint BuyPrice { get; set; }

	public uint SellPrice { get; set; }

	public byte BuyDemandLv { get; set; }

	public uint BuyFame { get; set; }

	public uint BuyGToken { get; set; }

	public uint BuyGBCoin { get; set; }

	public uint WeaponType { get; set; }

	public uint ArmorType { get; set; }

	public byte UpLimit { get; set; }

	public ushort BasicUpInx { get; set; }

	public ushort UpSucRatio { get; set; }

	public ushort UpLuckRatio { get; set; }

	public byte UpResource { get; set; }

	public ushort AddUpInx { get; set; }

	public uint ShieldAC { get; set; }

	public uint HitRatePlus { get; set; }

	public uint EvaRatePlus { get; set; }

	public uint MACriPlus { get; set; }

	public uint CriDamPlus { get; set; }

	public uint MagCriDamPlus { get; set; }

	public uint BT_Inx { get; set; }

	public string TitleName { get; set; }

	public string ItemUseSkill { get; set; }

	public string SetItemIndex { get; set; }

	public uint ItemFunc { get; set; }

	/// <summary>
	/// Get an item's <see cref="ItemInfo"/>.
	/// </summary>
	/// <param name="item">An item identification number.</param>
	/// <returns>An item's <see cref="ItemInfo"/>.</returns>
	public static (ItemInfo Client, ItemInfoServer Server) Read(ushort item)
	{
		return _cacheById.GetOrAdd(item, (
			Client: Asset.First<ItemInfo>("ItemInfo.shn", i => i.ID == item),
			Server: Asset.First<ItemInfoServer>("ItemInfoServer.shn", i => i.ID == item)));
	}
}