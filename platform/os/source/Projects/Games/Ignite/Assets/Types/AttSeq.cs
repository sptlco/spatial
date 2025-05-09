// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("MobAttackSequence\\*.txt\\AttSeq")]
public class AttSeq
{
	[JsonPropertyName("0")]
	public short Order { get; set; }

	[JsonPropertyName("1")]
	public string Attack { get; set; }
}
