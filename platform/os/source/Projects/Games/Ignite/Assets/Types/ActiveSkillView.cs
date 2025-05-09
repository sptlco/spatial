// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ActiveSkillView.shn")]
public class ActiveSkillView
{
	public ushort ID { get; set; }

	public string InxName { get; set; }

	public uint CancelCasting { get; set; }

	public byte TargetChange { get; set; }

	public uint IconIndex { get; set; }

	public string IconFile { get; set; }

	public uint R { get; set; }

	public uint G { get; set; }

	public uint B { get; set; }

	public uint CastingType { get; set; }

	public uint ActionType { get; set; }

	public string CasRdyAction { get; set; }

	public string CasAction { get; set; }

	public string SwingAction { get; set; }

	public uint ShoEfSpd { get; set; }

	public string ShoEffect { get; set; }

	public string ShoSnd { get; set; }

	public string LastAction { get; set; }

	public string LastEffectA { get; set; }

	public uint eLastEffPos { get; set; }

	public string LastEfASnd { get; set; }

	public string LastAreaEf { get; set; }

	public string LastAEfWhe { get; set; }

	public string LastAESnd { get; set; }

	public string DOTRageEft { get; set; }

	public string DOTRageEftSnd { get; set; }

	public string DOTRageEftLoop { get; set; }

	public string DOTRageEftLoopSnd { get; set; }

	public string Descript { get; set; }

	public uint uiDemandLv { get; set; }

	public string Function { get; set; }

	public byte HideHandItem { get; set; }
}
