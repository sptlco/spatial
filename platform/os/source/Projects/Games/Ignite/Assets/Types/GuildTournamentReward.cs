// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("GuildTournamentReward.shn")]
public class GuildTournamentReward
{
	public byte Rank { get; set; }

	public byte RewardGroup { get; set; }

	public uint RewardType { get; set; }

	public uint Value1 { get; set; }

	public uint Value2 { get; set; }

	public uint Value3 { get; set; }

	public uint IO_Str { get; set; }

	public uint IO_Con { get; set; }

	public uint IO_Dex { get; set; }

	public uint IO_Int { get; set; }

	public uint IO_Men { get; set; }
}
