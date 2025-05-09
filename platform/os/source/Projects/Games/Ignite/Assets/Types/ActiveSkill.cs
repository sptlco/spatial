// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ActiveSkill.shn")]
public class ActiveSkill
{
	public ushort ID { get; set; }

	public string InxName { get; set; }

	public string Name { get; set; }

	public uint Grade { get; set; }

	public uint Step { get; set; }

	public uint MaxStep { get; set; }

	public uint DemandType { get; set; }

	public string DemandSk { get; set; }

	public ushort UseItem { get; set; }

	public uint ItemNumber { get; set; }

	public uint ItemOption { get; set; }

	public ushort DemandItem1 { get; set; }

	public ushort DemandItem2 { get; set; }

	public uint SP { get; set; }

	public uint SPRate { get; set; }

	public uint HP { get; set; }

	public uint HPRate { get; set; }

	public uint LP { get; set; }

	public uint Range { get; set; }

	public uint First { get; set; }

	public uint Last { get; set; }

	public byte IsMovingSkill { get; set; }

	public ushort UsableDegree { get; set; }

	public ushort DirectionRotate { get; set; }

	public ushort SkillDegree { get; set; }

	public uint SkillTargetState { get; set; }

	public uint CastTime { get; set; }

	public uint DlyTime { get; set; }

	public uint DlyGroupNum { get; set; }

	public uint DlyTimeGroup { get; set; }

	public uint MinWC { get; set; }

	public uint MinWCRate { get; set; }

	public uint MaxWC { get; set; }

	public uint MaxWCRate { get; set; }

	public uint MinMA { get; set; }

	public uint MinMARate { get; set; }

	public uint MaxMA { get; set; }

	public uint MaxMARate { get; set; }

	public uint AC { get; set; }

	public uint MR { get; set; }

	public uint Area { get; set; }

	public uint TargetNumber { get; set; }

	public uint UseClass { get; set; }

	public string StaNameA { get; set; }

	public uint StaStrengthA { get; set; }

	public uint StaSucRateA { get; set; }

	public string StaNameB { get; set; }

	public uint StaStrengthB { get; set; }

	public uint StaSucRateB { get; set; }

	public string StaNameC { get; set; }

	public uint StaStrengthC { get; set; }

	public uint StaSucRateC { get; set; }

	public string StaNameD { get; set; }

	public uint StaStrengthD { get; set; }

	public uint StaSucRateD { get; set; }

	public uint[] nIMPT { get; set; }

	public uint[] nT0 { get; set; }

	public uint[] nT1 { get; set; }

	public uint[] nT2 { get; set; }

	public uint[] nT3 { get; set; }

	public uint EffectType { get; set; }

	public uint SpecialIndexA { get; set; }

	public uint SpecialValueA { get; set; }

	public uint SpecialIndexB { get; set; }

	public uint SpecialValueB { get; set; }

	public uint SpecialIndexC { get; set; }

	public uint SpecialValueC { get; set; }

	public uint SpecialIndexD { get; set; }

	public uint SpecialValueD { get; set; }

	public uint SpecialIndexE { get; set; }

	public uint SpecialValueE { get; set; }

	public string SkillClassifierA { get; set; }

	public string SkillClassifierB { get; set; }

	public string SkillClassifierC { get; set; }

	public byte CannotInside { get; set; }

	public byte DemandSoul { get; set; }

	public ushort HitID { get; set; }
}
