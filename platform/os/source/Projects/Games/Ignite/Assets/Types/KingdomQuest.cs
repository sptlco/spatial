// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("KingdomQuest.shn")]
public class KingdomQuest
{
	public ushort ID { get; set; }

	public string Title { get; set; }

	public ushort LimitTime { get; set; }

	public byte ST_Year { get; set; }

	public byte ST_Month { get; set; }

	public byte ST_Day { get; set; }

	public byte ST_Hour { get; set; }

	public byte ST_Minute { get; set; }

	public byte ST_Second { get; set; }

	public ushort StartWaitTime { get; set; }

	public byte NextStartMode { get; set; }

	public ushort NextStartDeleyMin { get; set; }

	public byte RepeatMode { get; set; }

	public ushort RepeatCount { get; set; }

	public byte MinLevel { get; set; }

	public byte MaxLevel { get; set; }

	public ushort MinPlayers { get; set; }

	public ushort MaxPlayers { get; set; }

	public byte PlayerRepeatMode { get; set; }

	public ushort PlayerRepeatCount { get; set; }

	public byte PlayerRevivalMode { get; set; }

	public byte PlayerRevivalCount { get; set; }

	public ushort DemandQuest { get; set; }

	public ushort DemandItem { get; set; }

	public byte DemandMobKill { get; set; }

	public uint RewardIndex { get; set; }

	public ushort[] MapLink { get; set; }

	public string ScriptLanguage { get; set; }

	public string InitValue { get; set; }

	public uint UseClass { get; set; }

	public byte[] DemandGender { get; set; }
}
