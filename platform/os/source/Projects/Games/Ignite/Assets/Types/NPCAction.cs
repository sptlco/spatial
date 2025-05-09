// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\NPCAction.txt\\NPCAction")]
public class NPCAction
{
	[JsonPropertyName("0")]
	public byte ActionID { get; set; }

	[JsonPropertyName("1")]
	public string ActionIndex { get; set; }

	[JsonPropertyName("2")]
	public string Type { get; set; }

	[JsonPropertyName("3")]
	public int X { get; set; }

	[JsonPropertyName("4")]
	public int Y { get; set; }
}
