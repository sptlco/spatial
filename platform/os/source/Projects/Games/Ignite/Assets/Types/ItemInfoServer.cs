// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ItemInfoServer.shn")]
public class ItemInfoServer
{
	public uint ID { get; set; }

	public string InxName { get; set; }

	public string MarketIndex { get; set; }

	public byte[] City { get; set; }

	public string DropGroupA { get; set; }

	public string DropGroupB { get; set; }

	public string RandomOptionDropGroup { get; set; }

	public uint Vanish { get; set; }

	public uint looting { get; set; }

	public ushort DropRateKilledByMob { get; set; }

	public ushort DropRateKilledByPlayer { get; set; }

	public uint ISET_Index { get; set; }

	public string ItemSort_Index { get; set; }

	public byte KQItem { get; set; }

	public byte PK_KQ_USE { get; set; }

	public byte KQ_Item_Drop { get; set; }

	public byte PreventAttack { get; set; }
}
