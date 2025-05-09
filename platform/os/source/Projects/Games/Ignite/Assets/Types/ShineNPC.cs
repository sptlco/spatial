// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\NPC.txt\\ShineNPC")]
public class ShineNPC
{
	[JsonPropertyName("0")]
	public string MobName { get; set; }

	[JsonPropertyName("1")]
	public string Map { get; set; }

	[JsonPropertyName("2")]
	public int CoordX { get; set; }

	[JsonPropertyName("3")]
	public int CoordY { get; set; }

	[JsonPropertyName("4")]
	public short Direct { get; set; }

	[JsonPropertyName("5")]
	public byte NPCMenu { get; set; }

	[JsonPropertyName("6")]
	public string Role { get; set; }

	[JsonPropertyName("7")]
	public string RoleArg0 { get; set; }
}
