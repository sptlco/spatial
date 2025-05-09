// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("KingdomQuestRew.shn")]
public class KingdomQuestRew
{
	public uint ID { get; set; }

	public string IndexString { get; set; }

	public string KQBoxItemIDX { get; set; }

	public ushort[] Reward { get; set; }

	public ushort[] RewardRate { get; set; }
}
