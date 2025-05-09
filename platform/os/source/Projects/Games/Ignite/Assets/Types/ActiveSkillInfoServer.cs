// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ActiveSkillInfoServer.shn")]
public class ActiveSkillInfoServer
{
	public ushort ID { get; set; }

	public string InxName { get; set; }

	public byte UsualAttack { get; set; }

	public uint SkilPyHitRate { get; set; }

	public uint SkilMaHitRate { get; set; }

	public uint PsySucRate { get; set; }

	public uint MagSucRate { get; set; }

	public byte StaLevel { get; set; }

	public uint DmgIncRate { get; set; }

	public ushort DmgIncValue { get; set; }

	public uint SkillHitType { get; set; }

	public byte ItremUseSkill { get; set; }

	public uint AggroPerDamage { get; set; }

	public uint AbsoluteAggro { get; set; }

	public byte AttackStart { get; set; }

	public byte AttackEnd { get; set; }

	public ushort SwingTime { get; set; }

	public ushort HitTime { get; set; }

	public byte AddSoul { get; set; }
}
