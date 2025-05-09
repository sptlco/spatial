// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("TutorialCharacterData.txt\\DEFAULT_ITEM")]
public class DEFAULT_ITEM
{
	[JsonPropertyName("0")]
	public int Class { get; set; }

	[JsonPropertyName("1")]
	public int ItemID { get; set; }

	[JsonPropertyName("2")]
	public int Lot { get; set; }
}
