// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\ItemOptions.txt\\Options")]
public class Options
{
	[JsonPropertyName("0")]
	public short OptionDegree { get; set; }

	[JsonPropertyName("1")]
	public string Type { get; set; }

	[JsonPropertyName("2")]
	public short _1 { get; set; }

	[JsonPropertyName("3")]
	public short _2 { get; set; }

	[JsonPropertyName("4")]
	public short _3 { get; set; }

	[JsonPropertyName("5")]
	public short _4 { get; set; }

	[JsonPropertyName("6")]
	public short _5 { get; set; }

	[JsonPropertyName("7")]
	public short _6 { get; set; }

	[JsonPropertyName("8")]
	public short _7 { get; set; }

	[JsonPropertyName("9")]
	public short _8 { get; set; }

	[JsonPropertyName("10")]
	public short _9 { get; set; }
}
