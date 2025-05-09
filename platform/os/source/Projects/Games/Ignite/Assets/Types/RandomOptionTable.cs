// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\RandomOptionTable.txt\\RandomOptionTable")]
public class RandomOptionTable
{
	[JsonPropertyName("0")]
	public string DropItemIndex { get; set; }

	[JsonPropertyName("1")]
	public byte OptionHide { get; set; }

	[JsonPropertyName("2")]
	public byte MinOpCount { get; set; }

	[JsonPropertyName("3")]
	public byte MaxOpCount { get; set; }

	[JsonPropertyName("4")]
	public short StrMin { get; set; }

	[JsonPropertyName("5")]
	public short StrMax { get; set; }

	[JsonPropertyName("6")]
	public short ConMin { get; set; }

	[JsonPropertyName("7")]
	public short ConMax { get; set; }

	[JsonPropertyName("8")]
	public short DexMin { get; set; }

	[JsonPropertyName("9")]
	public short DexMax { get; set; }

	[JsonPropertyName("10")]
	public short IntMin { get; set; }

	[JsonPropertyName("11")]
	public short IntMax { get; set; }

	[JsonPropertyName("12")]
	public short MenMin { get; set; }

	[JsonPropertyName("13")]
	public short MenMax { get; set; }

	[JsonPropertyName("14")]
	public byte CheckSum { get; set; }
}
