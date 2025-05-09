// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("RandomOption.shn")]
public class RandomOption
{
	public string DropItemIndex { get; set; }

	public uint RandomOptionType { get; set; }

	public uint Min { get; set; }

	public uint Max { get; set; }

	public uint TypeDropRate { get; set; }
}
