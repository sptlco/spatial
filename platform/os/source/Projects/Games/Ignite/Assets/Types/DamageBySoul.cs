// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\DamageBySoul.txt\\DamageBySoul")]
public class DamageBySoul
{
	[JsonPropertyName("0")]
	public short DemandSoul { get; set; }

	[JsonPropertyName("1")]
	public short Soul00 { get; set; }
	
	[JsonPropertyName("2")]
	public short Soul01 { get; set; }

	[JsonPropertyName("3")]
	public short Soul02 { get; set; }

	[JsonPropertyName("4")]
	public short Soul03 { get; set; }

	[JsonPropertyName("5")]
	public short Soul04 { get; set; }

	[JsonPropertyName("6")]
	public short Soul05 { get; set; }

	[JsonPropertyName("7")]
	public short Soul06 { get; set; }

	[JsonPropertyName("8")]
	public short Soul07 { get; set; }
}
