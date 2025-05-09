// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MiniHouseDummy.shn")]
public class MiniHouseDummy
{
	public ushort No { get; set; }

	public string Index { get; set; }

	public string IconFileName { get; set; }

	public ushort nIconNum { get; set; }

	public ushort HPTick { get; set; }

	public ushort SPTick { get; set; }

	public ushort HPRecovery { get; set; }

	public ushort SPRecovery { get; set; }

	public ushort Casting { get; set; }

	public byte Slot { get; set; }

	public string HouseAType { get; set; }

	public uint HouseALoc { get; set; }

	public string HouseBType { get; set; }

	public uint HouseBLoc { get; set; }

	public string HouseCType { get; set; }

	public uint HouseCLoc { get; set; }
}
