// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("NpcSchedule.shn")]
public class NpcSchedule
{
	public string Mob_Inx { get; set; }

	public ushort NS_Year { get; set; }

	public byte NS_Month { get; set; }

	public byte NS_Day { get; set; }

	public byte NS_Hour { get; set; }

	public byte NS_Minute { get; set; }

	public ushort NS_CycleHour { get; set; }

	public byte NS_LifeHour { get; set; }

	public byte NS_IsMsg { get; set; }
}
