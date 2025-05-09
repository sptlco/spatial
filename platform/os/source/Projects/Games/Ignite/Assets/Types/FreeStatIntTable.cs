// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\ChrCommon.txt\\FreeStatIntTable")]
public class FreeStatIntTable
{
	[JsonPropertyName("0")]
	public byte Stat { get; set; }

	[JsonPropertyName("1")]
	public short MAAbsolute { get; set; }

	[JsonPropertyName("2")]
	public byte CheckSum { get; set; }
}
