// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\MiscDataTable.txt\\ExpandSkill")]
public class ExpandSkill
{
	[JsonPropertyName("0")]
	public byte Serial { get; set; }

	[JsonPropertyName("1")]
	public string Skill { get; set; }

	[JsonPropertyName("2")]
	public string Condition { get; set; }

	[JsonPropertyName("3")]
	public short Dmg { get; set; }

	[JsonPropertyName("4")]
	public string AbState { get; set; }

	[JsonPropertyName("5")]
	public short Critical { get; set; }

	[JsonPropertyName("6")]
	public byte CheckSum { get; set; }
}
