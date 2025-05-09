// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("Gather.shn")]
public class Gather
{
	public ushort GatherID { get; set; }

	public string Index { get; set; }

	public uint Type { get; set; }

	public string NeededTool0 { get; set; }

	public string NeededTool1 { get; set; }

	public string NeededTool2 { get; set; }

	public string EqipItemView { get; set; }

	public uint AniNumber { get; set; }

	public uint Gauge { get; set; }
}
