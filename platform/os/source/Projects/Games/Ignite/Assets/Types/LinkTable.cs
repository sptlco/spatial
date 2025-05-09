// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\NPC.txt\\LinkTable")]
public class LinkTable
{
	[JsonPropertyName("0")]
	public string Argument { get; set; }

	[JsonPropertyName("1")]
	public string MapServer { get; set; }

	[JsonPropertyName("2")]
	public string MapClient { get; set; }

	[JsonPropertyName("3")]
	public int CoordX { get; set; }

	[JsonPropertyName("4")]
	public int CoordY { get; set; }

	[JsonPropertyName("5")]
	public short Direct { get; set; }

	[JsonPropertyName("6")]
	public byte Party { get; set; }
}
