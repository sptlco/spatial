// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

/// <summary>
/// Default character data.
/// </summary>
[Name("DefaultCharacterData.txt\\CHARACTER")]
public class CHARACTER
{
	/// <summary>
	/// The character's class.
	/// </summary>
	[JsonPropertyName("0")]
	public int Class { get; set; }

	/// <summary>
	/// The character's map.
	/// </summary>
	[JsonPropertyName("1")]
	public byte Map { get; set; }

	/// <summary>
	/// The X-coordinate of the character's position.
	/// </summary>
	[JsonPropertyName("2")]
	public int PX { get; set; }

	/// <summary>
	/// The Y-coordinate of the character's position.
	/// /// </summary>
	[JsonPropertyName("3")]
	public int PY { get; set; }

	/// <summary>
	/// The character's health points.
	/// </summary>
	[JsonPropertyName("4")]
	public int HP { get; set; }

	/// <summary>
	/// The character's mana points.
	/// </summary>
	[JsonPropertyName("5")]
	public int SP { get; set; }

	/// <summary>
	/// The character's HP stone count.
	/// </summary>
	[JsonPropertyName("6")]
	public int HPSoul { get; set; }

	/// <summary>
	/// The character's SP stone count.
	/// </summary>
	[JsonPropertyName("7")]
	public int SPSoul { get; set; }

	/// <summary>
	/// The character's copper balance.
	/// </summary>
	[JsonPropertyName("8")]
	public int Money { get; set; }

	/// <summary>
	/// The character's level.
	/// </summary>
	[JsonPropertyName("9")]
	public int InitLV { get; set; }

	/// <summary>
	/// The character's experience points.
	/// </summary>
	[JsonPropertyName("10")]
	public string InitEXP { get; set; }
}
