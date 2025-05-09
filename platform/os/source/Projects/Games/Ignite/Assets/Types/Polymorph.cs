// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\MiscDataTable.txt\\Polymorph")]
public class Polymorph
{
	[JsonPropertyName("0")]
	public byte Serial { get; set; }

	[JsonPropertyName("1")]
	public string Skill { get; set; }

	[JsonPropertyName("2")]
	public string MobIndex { get; set; }

	[JsonPropertyName("3")]
	public byte CanMove { get; set; }

	[JsonPropertyName("4")]
	public byte CanAttack { get; set; }

	[JsonPropertyName("5")]
	public byte CheckSum { get; set; }
}
