// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("MobRoam\\*.txt\\Roaming")]
public class Roaming
{
	[JsonPropertyName("0")]
	public short ID { get; set; }

	[JsonPropertyName("1")]
	public int X { get; set; }

	[JsonPropertyName("2")]
	public int Y { get; set; }

	[JsonPropertyName("3")]
	public string EventIndex { get; set; }
}
