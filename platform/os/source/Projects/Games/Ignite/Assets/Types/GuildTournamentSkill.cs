// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("GuildTournamentSkill.shn")]
public class GuildTournamentSkill
{
	public ushort MAP_TYPE { get; set; }

	public ushort Index { get; set; }

	public ushort DeathPoint { get; set; }

	public string StaName { get; set; }

	public uint TargetType { get; set; }

	public uint DlyTime { get; set; }
}
