// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\MobChat.txt\\DEAD")]
public class DEAD
{
	[JsonPropertyName("0")]
	public string MobIndex { get; set; }

	[JsonPropertyName("1")]
	public int Rate0 { get; set; }

	[JsonPropertyName("2")]
	public int Rate1 { get; set; }

	[JsonPropertyName("3")]
	public string Script0 { get; set; }

	[JsonPropertyName("4")]
	public string Script1 { get; set; }

	[JsonPropertyName("5")]
	public string Script2 { get; set; }

	[JsonPropertyName("6")]
	public string Script3 { get; set; }
}
