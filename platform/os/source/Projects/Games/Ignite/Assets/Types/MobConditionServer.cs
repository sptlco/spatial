// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MobConditionServer.shn")]
public class MobConditionServer
{
	public string MobInx { get; set; }

	public byte AniLv { get; set; }

	public uint MC_Type { get; set; }

	public uint MC_ValueMin { get; set; }

	public uint MC_ValueMax { get; set; }
}
