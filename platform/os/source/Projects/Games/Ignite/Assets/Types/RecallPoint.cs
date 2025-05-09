// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\RecallCoord.txt\\RecallPoint")]
public class RecallPoint
{
	[JsonPropertyName("0")]
	public string ItemIndex { get; set; }

	[JsonPropertyName("1")]
	public int ItemIdent { get; set; }

	[JsonPropertyName("2")]
	public string MapName { get; set; }

	[JsonPropertyName("3")]
	public short LinkX { get; set; }

	[JsonPropertyName("4")]
	public short LinkY { get; set; }
}
