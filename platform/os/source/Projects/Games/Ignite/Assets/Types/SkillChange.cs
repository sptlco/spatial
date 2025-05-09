// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("MobAttackSequence\\*.txt\\SkillChange")]
public class SkillChange
{
	[JsonPropertyName("0")]
	public string Condition { get; set; }

	[JsonPropertyName("1")]
	public string Value { get; set; }

	[JsonPropertyName("2")]
	public string From { get; set; }

	[JsonPropertyName("3")]
	public string To { get; set; }
}
