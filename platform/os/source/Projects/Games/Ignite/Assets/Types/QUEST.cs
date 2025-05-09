// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("DefaultCharacterData.txt\\QUEST")]
public class QUEST
{
	[JsonPropertyName("0")]
	public int Class { get; set; }

	[JsonPropertyName("1")]
	public int QuestID { get; set; }
}
