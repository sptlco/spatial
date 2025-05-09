// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("CollectCardReward.shn")]
public class CollectCardReward
{
	public ushort CC_RewardID { get; set; }

	public uint CC_CardRewardType { get; set; }

	public ushort CC_CardLot { get; set; }

	public string CC_RewardItemInx { get; set; }

	public ushort CC_RewardLot { get; set; }
}
