// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("DefaultCharacterData.txt\\SKILL")]
public class SKILL
{
	[JsonPropertyName("0")]
	public int Class { get; set; }

	[JsonPropertyName("1")]
	public int SkillID { get; set; }
}
