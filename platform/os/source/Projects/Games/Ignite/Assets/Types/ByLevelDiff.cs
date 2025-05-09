// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\ExpRecalculation.txt\\ByLevelDiff")]
public class ByLevelDiff
{
	[JsonPropertyName("0")]
	public short LevelDiff { get; set; }

	[JsonPropertyName("1")]
	public short Bonus { get; set; }
}
