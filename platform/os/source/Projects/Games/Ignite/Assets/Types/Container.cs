// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\TreasureReward.txt\\Container")]
public class Container
{
	[JsonPropertyName("0")]
	public int ItemID { get; set; }

	[JsonPropertyName("1")]
	public short Index { get; set; }

	[JsonPropertyName("2")]
	public string CardInx { get; set; }

	[JsonPropertyName("3")]
	public short MinLot { get; set; }

	[JsonPropertyName("4")]
	public short MaxLot { get; set; }

	[JsonPropertyName("5")]
	public string DummyInx { get; set; }
}
