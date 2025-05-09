// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("DiceGame.shn")]
public class DiceGame
{
	public ushort ItemID { get; set; }

	public ushort UseMinLv { get; set; }

	public ushort GetSysRate { get; set; }

	public ushort GetMasterRate { get; set; }

	public uint MinGetMoney { get; set; }

	public uint MaxBetMoney { get; set; }

	public ushort CastTime { get; set; }

	public ushort DelayTime { get; set; }

	public uint[] WinCode { get; set; }

	public uint[] LoseCode { get; set; }
}
