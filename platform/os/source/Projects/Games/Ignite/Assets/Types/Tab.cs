// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("NPCItemList\\*.txt\\Tab*")]
public class Tab
{
	[JsonPropertyName("0")]
	public byte Rec { get; set; }

	[JsonPropertyName("1")]
	public string Column00 { get; set; }

	[JsonPropertyName("2")]
	public string Column01 { get; set; }

	[JsonPropertyName("3")]
	public string Column02 { get; set; }

	[JsonPropertyName("4")]
	public string Column03 { get; set; }

	[JsonPropertyName("5")]
	public string Column04 { get; set; }

	[JsonPropertyName("6")]
	public string Column05 { get; set; }
}
