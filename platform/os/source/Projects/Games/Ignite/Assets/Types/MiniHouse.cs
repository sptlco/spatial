// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MiniHouse.shn")]
public class MiniHouse
{
	public ushort Handle { get; set; }

	public string ItemID { get; set; }

	public string DummyType { get; set; }

	public string Backimage { get; set; }

	public ushort KeepTime_Hour { get; set; }

	public ushort HPTick { get; set; }

	public ushort SPTick { get; set; }

	public ushort HPRecovery { get; set; }

	public ushort SPRecovery { get; set; }

	public ushort Casting { get; set; }

	public byte Slot { get; set; }
}
