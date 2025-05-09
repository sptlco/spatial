// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\Field.txt\\GuildTournamentMap")]
public class GuildTournamentMap
{
	[JsonPropertyName("0")]
	public string MapIDClient { get; set; }

	[JsonPropertyName("1")]
	public byte GTMapNo { get; set; }

	[JsonPropertyName("2")]
	public int StoneX { get; set; }

	[JsonPropertyName("3")]
	public int StoneY { get; set; }

	[JsonPropertyName("4")]
	public short StoneDir { get; set; }

	[JsonPropertyName("5")]
	public int GldARgX { get; set; }

	[JsonPropertyName("6")]
	public int GldARgY { get; set; }

	[JsonPropertyName("7")]
	public int GldBRgX { get; set; }

	[JsonPropertyName("8")]
	public int GldBRgY { get; set; }

	[JsonPropertyName("9")]
	public int DoorAX0 { get; set; }

	[JsonPropertyName("10")]
	public int DoorAY0 { get; set; }

	[JsonPropertyName("11")]
	public int DoorAD0 { get; set; }

	[JsonPropertyName("12")]
	public string DoorABlock0 { get; set; }

	[JsonPropertyName("13")]
	public int DoorBX0 { get; set; }

	[JsonPropertyName("14")]
	public int DoorBY0 { get; set; }

	[JsonPropertyName("15")]
	public int DoorBD0 { get; set; }

	[JsonPropertyName("16")]
	public string DoorBBlock0 { get; set; }

	[JsonPropertyName("17")]
	public int DoorAX1 { get; set; }

	[JsonPropertyName("18")]
	public int DoorAY1 { get; set; }

	[JsonPropertyName("19")]
	public int DoorAD1 { get; set; }

	[JsonPropertyName("20")]
	public string DoorABlock1 { get; set; }

	[JsonPropertyName("21")]
	public int DoorBX1 { get; set; }

	[JsonPropertyName("22")]
	public int DoorBY1 { get; set; }

	[JsonPropertyName("23")]
	public int DoorBD1 { get; set; }

	[JsonPropertyName("24")]
	public string DoorBBlock1 { get; set; }

	[JsonPropertyName("25")]
	public int DoorAX2 { get; set; }

	[JsonPropertyName("26")]
	public int DoorAY2 { get; set; }

	[JsonPropertyName("27")]
	public int DoorAD2 { get; set; }

	[JsonPropertyName("28")]
	public string DoorABlock2 { get; set; }

	[JsonPropertyName("29")]
	public int DoorBX2 { get; set; }

	[JsonPropertyName("30")]
	public int DoorBY2 { get; set; }

	[JsonPropertyName("31")]
	public int DoorBD2 { get; set; }

	[JsonPropertyName("32")]
	public string DoorBBlock2 { get; set; }

	[JsonPropertyName("33")]
	public byte CheckSum { get; set; }
}
