// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\TreasureReward.txt\\Content")]
public class Content
{
	[JsonPropertyName("0")]
	public int ItemID { get; set; }

	[JsonPropertyName("1")]
	public short Index { get; set; }

	[JsonPropertyName("2")]
	public string RewardInx { get; set; }

	[JsonPropertyName("3")]
	public short RewardLot { get; set; }

	[JsonPropertyName("4")]
	public byte UpgradeLow { get; set; }

	[JsonPropertyName("5")]
	public byte UpgradeHigh { get; set; }

	[JsonPropertyName("6")]
	public int RewardRate { get; set; }

	[JsonPropertyName("7")]
	public short CheckSum { get; set; }
}
