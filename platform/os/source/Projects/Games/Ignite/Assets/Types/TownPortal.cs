// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("TownPortal.shn")]
public class TownPortal
{
	public byte Index { get; set; }

	public byte MinLevel { get; set; }

	public byte TP_GroupNo { get; set; }

	public string MapName { get; set; }

	public uint X { get; set; }

	public uint Y { get; set; }
}
