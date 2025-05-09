// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types.PlainText;

[Name("World\\MiscDataTable.txt\\AbStateReset")]
public class AbStateReset
{
	[JsonPropertyName("0")]
	public byte Serial { get; set; }

	[JsonPropertyName("1")]
	public string AbState { get; set; }

	[JsonPropertyName("2")]
	public byte Run { get; set; }

	[JsonPropertyName("3")]
	public byte Walk { get; set; }

	[JsonPropertyName("4")]
	public byte Attack { get; set; }

	[JsonPropertyName("5")]
	public byte Attacked { get; set; }

	[JsonPropertyName("6")]
	public string EquipWhere { get; set; }

	[JsonPropertyName("7")]
	public string EquipClass { get; set; }

	[JsonPropertyName("8")]
	public byte CheckSum { get; set; }
}
