// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\ItemDropGroup.txt\\ItemDropGroup")]
public class ItemDropGroup
{
	[JsonPropertyName("0")]
	public string ItemGroupIdx { get; set; }

	[JsonPropertyName("1")]
	public string ItemID { get; set; }

	[JsonPropertyName("2")]
	public short MinQtty { get; set; }

	[JsonPropertyName("3")]
	public short MaxQtty { get; set; }

	[JsonPropertyName("4")]
	public short Upgrade00 { get; set; }

	[JsonPropertyName("5")]
	public short Upgrade01 { get; set; }

	[JsonPropertyName("6")]
	public short Upgrade02 { get; set; }

	[JsonPropertyName("7")]
	public short Upgrade03 { get; set; }

	[JsonPropertyName("8")]
	public short Upgrade04 { get; set; }

	[JsonPropertyName("9")]
	public short Upgrade05 { get; set; }

	[JsonPropertyName("10")]
	public short Upgrade06 { get; set; }

	[JsonPropertyName("11")]
	public short Upgrade07 { get; set; }

	[JsonPropertyName("12")]
	public short Upgrade08 { get; set; }

	[JsonPropertyName("13")]
	public short Upgrade09 { get; set; }

	[JsonPropertyName("14")]
	public short Upgrade10 { get; set; }

	[JsonPropertyName("15")]
	public short Upgrade11 { get; set; }

	[JsonPropertyName("16")]
	public short Upgrade12 { get; set; }

	[JsonPropertyName("17")]
	public short Upgrade13 { get; set; }

	[JsonPropertyName("18")]
	public short Upgrade14 { get; set; }

	[JsonPropertyName("19")]
	public short Upgrade15 { get; set; }

	[JsonPropertyName("20")]
	public int CheckSum { get; set; }
}
