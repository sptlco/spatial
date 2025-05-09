// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\MiscDataTable.txt\\Neutralization")]
public class Neutralization
{
	[JsonPropertyName("0")]
	public byte Serial { get; set; }

	[JsonPropertyName("1")]
	public string Skill { get; set; }

	[JsonPropertyName("2")]
	public short Weapon { get; set; }

	[JsonPropertyName("3")]
	public short Shield { get; set; }

	[JsonPropertyName("4")]
	public short Body { get; set; }

	[JsonPropertyName("5")]
	public short Leg { get; set; }

	[JsonPropertyName("6")]
	public byte CheckSum { get; set; }
}
