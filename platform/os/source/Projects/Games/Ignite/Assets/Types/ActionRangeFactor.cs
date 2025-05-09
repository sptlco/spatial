// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ActionRangeFactor.shn")]
public class ActionRangeFactor
{
	public uint ActionRangeIndex { get; set; }

	public uint RangeType { get; set; }

	public ushort RangeStart { get; set; }

	public ushort RangeEnd { get; set; }
}
