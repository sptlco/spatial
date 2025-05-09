// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MobRegenAni.shn")]
public class MobRegenAni
{
	public string MobIDX { get; set; }

	public ushort RegenTime { get; set; }

	public string GroupAbStateIDX { get; set; }

	public byte IsAggro { get; set; }
}
