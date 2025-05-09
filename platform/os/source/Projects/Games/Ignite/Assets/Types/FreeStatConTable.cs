// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\ChrCommon.txt\\FreeStatConTable")]
public class FreeStatConTable
{
	[JsonPropertyName("0")]
	public byte Stat { get; set; }

	[JsonPropertyName("1")]
	public short ACAbsolute { get; set; }

	[JsonPropertyName("2")]
	public short BlockRate { get; set; }

	[JsonPropertyName("3")]
	public short MaxHP { get; set; }

	[JsonPropertyName("4")]
	public byte CheckSum { get; set; }
}
