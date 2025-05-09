// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\DamageByAngle.txt\\DamageByAngle_Mob")]
public class DamageByAngle_Mob
{
	[JsonPropertyName("0")]
	public short DamagedAngle { get; set; }

	[JsonPropertyName("1")]
	public short DamageRate { get; set; }

	[JsonPropertyName("2")]
	public short CheckSum { get; set; }
}
