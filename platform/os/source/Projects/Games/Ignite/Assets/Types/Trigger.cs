// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("MobSetting\\Action\\*.txt\\Trigger")]
public class Trigger
{
	[JsonPropertyName("0")]
	public int ConditionID { get; set; }

	[JsonPropertyName("1")]
	public int ActionID { get; set; }
}
