// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("MobRegen\\*.txt\\MobRegen")]
public class MobRegen
{
	[JsonPropertyName("0")]
	public string RegenIndex { get; set; }

	[JsonPropertyName("1")]
	public string MobIndex { get; set; }

	[JsonPropertyName("2")]
	public byte MobNum { get; set; }

	[JsonPropertyName("3")]
	public byte KillNum { get; set; }

	[JsonPropertyName("4")]
	public int RegStandard { get; set; }

	[JsonPropertyName("5")]
	public int RegMin { get; set; }

	[JsonPropertyName("6")]
	public int RegMax { get; set; }

	[JsonPropertyName("7")]
	public int RegDelta0 { get; set; }

	[JsonPropertyName("8")]
	public int RegSec0 { get; set; }

	[JsonPropertyName("9")]
	public int RegDelta1 { get; set; }

	[JsonPropertyName("10")]
	public int RegSec1 { get; set; }

	[JsonPropertyName("11")]
	public int RegDelta2 { get; set; }

	[JsonPropertyName("12")]
	public int RegSec2 { get; set; }

	[JsonPropertyName("13")]
	public int RegDelta3 { get; set; }

	[JsonPropertyName("14")]
	public int RegSec3 { get; set; }

	[JsonPropertyName("15")]
	public int RegDelta4 { get; set; }
}
