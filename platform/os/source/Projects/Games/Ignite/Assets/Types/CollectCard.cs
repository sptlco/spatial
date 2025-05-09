// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("CollectCard.shn")]
public class CollectCard
{
	public ushort CC_CardID { get; set; }

	public string CC_ItemInx { get; set; }

	public uint CC_CardGradeType { get; set; }

	public uint CC_CardMobGroup { get; set; }
}
