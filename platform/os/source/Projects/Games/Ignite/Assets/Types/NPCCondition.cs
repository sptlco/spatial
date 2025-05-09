// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\NPCAction.txt\\NPCCondition")]
public class NPCCondition
{
	[JsonPropertyName("0")]
	public byte ConditionID { get; set; }

	[JsonPropertyName("1")]
	public string ConditionA { get; set; }

	[JsonPropertyName("2")]
	public string TypeA { get; set; }

	[JsonPropertyName("3")]
	public int AX { get; set; }

	[JsonPropertyName("4")]
	public int AY { get; set; }

	[JsonPropertyName("5")]
	public string ConditionB { get; set; }

	[JsonPropertyName("6")]
	public string TypeB { get; set; }

	[JsonPropertyName("7")]
	public int BX { get; set; }

	[JsonPropertyName("8")]
	public int BY { get; set; }
}
