// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("DefaultCharacterData.txt\\ITEM")]
public class ITEM
{
	[JsonPropertyName("0")]
	public int Class { get; set; }

	[JsonPropertyName("1")]
	public int ItemID { get; set; }

	[JsonPropertyName("2")]
	public int Lot { get; set; }
}
