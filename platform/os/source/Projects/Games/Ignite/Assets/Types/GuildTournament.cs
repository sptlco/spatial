// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("GuildTournament.shn")]
public class GuildTournament
{
	public byte isActive { get; set; }

	public byte Weeks { get; set; }

	public byte Week { get; set; }

	public byte Hour { get; set; }

	public byte Minute { get; set; }

	public byte TermHour { get; set; }

	public byte TermMinute { get; set; }

	public ushort MatchCycleMin { get; set; }

	public ushort ExploerTimeMin { get; set; }

	public ushort WaitPlayTimeSec { get; set; }

	public ushort PlayTime { get; set; }

	public ushort Deadline { get; set; }

	public ushort MatchDelay { get; set; }

	public ushort Match_161 { get; set; }

	public ushort Match_162 { get; set; }

	public ushort Match_8 { get; set; }

	public ushort Match_4 { get; set; }

	public ushort Match_2 { get; set; }
}
