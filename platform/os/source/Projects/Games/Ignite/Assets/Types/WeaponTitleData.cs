// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("WeaponTitleData.shn")]
public class WeaponTitleData
{
	public ushort MobID { get; set; }

	public byte Level { get; set; }

	public string Prefix { get; set; }

	public string Suffix { get; set; }

	public uint MobKillCount { get; set; }

	public ushort MinAdd { get; set; }

	public ushort MaxAdd { get; set; }

	public byte SP1_Reference { get; set; }

	public ushort SP1_Type { get; set; }

	public uint SP1_Value { get; set; }

	public byte SP2_Reference { get; set; }

	public ushort SP2_Type { get; set; }

	public uint SP2_Value { get; set; }

	public byte SP3_Reference { get; set; }

	public ushort SP3_Type { get; set; }

	public uint SP3_Value { get; set; }
}
