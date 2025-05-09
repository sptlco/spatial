// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MoverMain.shn")]
public class MoverMain
{
	public uint MoverID { get; set; }

	public string MoverIDX { get; set; }

	public uint CastingTime { get; set; }

	public uint CoolTime { get; set; }

	public ushort RunSpeed { get; set; }

	public ushort WalkSpeed { get; set; }

	public ushort DurationHour { get; set; }

	public byte MaxCharSlot { get; set; }
}
