// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("DefaultCharacterData.txt\\MAXSHAPE")]
public class MAXSHAPE
{
	[JsonPropertyName("0")]
	public int Class { get; set; }

	[JsonPropertyName("1")]
	public int MaxFace { get; set; }

	[JsonPropertyName("2")]
	public int MaxHair { get; set; }

	[JsonPropertyName("3")]
	public int MaxHairColor { get; set; }
}
