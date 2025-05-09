// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("SpamerPenalty.shn")]
public class SpamerPenalty
{
	public byte PenaltyLv { get; set; }

	public ushort ChatBlockTime { get; set; }

	public ushort ProbateTime { get; set; }
}
