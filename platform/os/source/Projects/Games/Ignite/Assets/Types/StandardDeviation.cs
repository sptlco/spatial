// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\ExpRecalculation.txt\\StandardDeviation")]
public class StandardDeviation
{
	[JsonPropertyName("0")]
	public short LowerBound100 { get; set; }

	[JsonPropertyName("1")]
	public short HandicapRate { get; set; }
}
