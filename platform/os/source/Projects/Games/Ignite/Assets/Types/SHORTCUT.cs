// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("DefaultCharacterData.txt\\SHORTCUT")]
public class SHORTCUT
{
	[JsonPropertyName("0")]
	public int Class { get; set; }

	[JsonPropertyName("1")]
	public int nSlotNo { get; set; }

	[JsonPropertyName("2")]
	public int nCodeNo { get; set; }

	[JsonPropertyName("3")]
	public int nValue { get; set; }
}
