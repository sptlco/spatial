// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\Field.txt\\InstanceDungeon")]
public class InstanceDungeon
{
	[JsonPropertyName("0")]
	public string Argument { get; set; }

	[JsonPropertyName("1")]
	public byte IDNo { get; set; }

	[JsonPropertyName("2")]
	public byte ZoneNumber { get; set; }

	[JsonPropertyName("3")]
	public string MapIDClient { get; set; }

	[JsonPropertyName("4")]
	public string ScriptName { get; set; }

	[JsonPropertyName("5")]
	public byte ModeIDLv { get; set; }

	[JsonPropertyName("6")]
	public byte EntranceType { get; set; }

	[JsonPropertyName("7")]
	public byte Guild { get; set; }

	[JsonPropertyName("8")]
	public byte Individual { get; set; }

	[JsonPropertyName("9")]
	public short NeedQuest { get; set; }

	[JsonPropertyName("10")]
	public string NeedItem { get; set; }

	[JsonPropertyName("11")]
	public byte Consume { get; set; }

	[JsonPropertyName("12")]
	public byte LimitTime { get; set; }

	[JsonPropertyName("13")]
	public byte MaxUseChr { get; set; }

	[JsonPropertyName("14")]
	public byte CheckSum { get; set; }

	[JsonPropertyName("15")]
	public short Spacer { get; set; }
}
