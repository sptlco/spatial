// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\ChrCommon.txt\\FreeStatDexTable")]
public class FreeStatDexTable
{
	[JsonPropertyName("0")]
	public byte Stat { get; set; }

	[JsonPropertyName("1")]
	public short THRate { get; set; }

	[JsonPropertyName("2")]
	public short TBRate { get; set; }

	[JsonPropertyName("3")]
	public byte CheckSum { get; set; }
}
