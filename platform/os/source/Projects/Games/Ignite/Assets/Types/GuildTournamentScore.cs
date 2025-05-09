// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("GuildTournamentScore.shn")]
public class GuildTournamentScore
{
	public ushort MAP_TYPE { get; set; }

	public ushort[] GradeScore { get; set; }
}
