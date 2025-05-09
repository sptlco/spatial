// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MobInfo.shn")]
public class MobInfo
{
	public ushort ID { get; set; }

	public string InxName { get; set; }

	public string Name { get; set; }

	public uint Level { get; set; }

	public uint MaxHP { get; set; }

	public uint WalkSpeed { get; set; }

	public uint RunSpeed { get; set; }

	public byte IsNPC { get; set; }

	public uint Size { get; set; }

	public uint WeaponType { get; set; }

	public uint ArmorType { get; set; }

	public uint GradeType { get; set; }

	public uint Type { get; set; }

	public byte IsPlayerSide { get; set; }

	public uint AbsoluteSize { get; set; }
}
