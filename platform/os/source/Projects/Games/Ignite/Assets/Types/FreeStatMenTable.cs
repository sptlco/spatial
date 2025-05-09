// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\ChrCommon.txt\\FreeStatMenTable")]
public class FreeStatMenTable
{
	[JsonPropertyName("0")]
	public byte Stat { get; set; }

	[JsonPropertyName("1")]
	public short MRAbsolute { get; set; }

	[JsonPropertyName("2")]
	public short CriRate { get; set; }

	[JsonPropertyName("3")]
	public short MaxSP { get; set; }

	[JsonPropertyName("4")]
	public byte CheckSum { get; set; }
}
