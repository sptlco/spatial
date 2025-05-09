// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\ChrCommon.txt\\FreeStatStrTable")]
public class FreeStatStrTable
{
	[JsonPropertyName("0")]
	public byte Stat { get; set; }

	[JsonPropertyName("1")]
	public short WCAbsolute { get; set; }

	[JsonPropertyName("2")]
	public byte CheckSum { get; set; }
}
