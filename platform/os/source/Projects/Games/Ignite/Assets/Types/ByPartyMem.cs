// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\ExpRecalculation.txt\\ByPartyMem")]
public class ByPartyMem
{
	[JsonPropertyName("0")]
	public byte PartyMember { get; set; }

	[JsonPropertyName("1")]
	public short Bonus { get; set; }
}
