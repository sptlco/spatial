// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ShineReward.shn")]
public class ShineReward
{
	public ushort RewardHandle { get; set; }

	public byte RewardType { get; set; }

	public string Argument { get; set; }

	public uint Quantity { get; set; }

	public ushort[] Upgrade { get; set; }

	public ushort OptionDegree { get; set; }

	public uint TitleDegree { get; set; }
}
