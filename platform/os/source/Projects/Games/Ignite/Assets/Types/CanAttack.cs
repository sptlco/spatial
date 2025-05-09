// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\SubLayerInteract.txt\\CanAttack")]
public class CanAttack
{
	[JsonPropertyName("0")]
	public byte CheckSumA { get; set; }

	[JsonPropertyName("1")]
	public string DoNotCare { get; set; }

	[JsonPropertyName("2")]
	public byte Base { get; set; }

	[JsonPropertyName("3")]
	public byte RangerStealth { get; set; }

	[JsonPropertyName("4")]
	public byte Observer { get; set; }

	[JsonPropertyName("5")]
	public byte AdminHide { get; set; }

	[JsonPropertyName("6")]
	public byte GMDoor { get; set; }

	[JsonPropertyName("7")]
	public byte GMPlayer { get; set; }

	[JsonPropertyName("8")]
	public byte CheckSumB { get; set; }
}
