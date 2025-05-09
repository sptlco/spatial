// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("PassiveSkill.shn")]
public class PassiveSkill
{
	public ushort ID { get; set; }

	public string InxName { get; set; }

	public string Name { get; set; }

	public uint WeaponMastery { get; set; }

	public string DemandSk { get; set; }

	public uint MstRtTmp { get; set; }

	public uint MstRtSword1 { get; set; }

	public uint MstRtHammer1 { get; set; }

	public uint MstRtSword2 { get; set; }

	public uint MstRtAxe2 { get; set; }

	public uint MstRtMace1 { get; set; }

	public uint MstRtBow2 { get; set; }

	public uint MstRtCrossBow2 { get; set; }

	public uint MstRtWand2 { get; set; }

	public uint MstRtStaff2 { get; set; }

	public uint MstRtClaw { get; set; }

	public uint MstRtDSword { get; set; }

	public uint MstPlTmp { get; set; }

	public uint MstPlSword1 { get; set; }

	public uint MstPlHammer1 { get; set; }

	public uint MstPlSword2 { get; set; }

	public uint MstPlAxe2 { get; set; }

	public uint MstPlMace1 { get; set; }

	public uint MstPlBow2 { get; set; }

	public uint MstPlCrossBow2 { get; set; }

	public uint MstPlWand2 { get; set; }

	public uint MstPlStaff2 { get; set; }

	public uint MstPlClaw { get; set; }

	public uint MstPlDSword { get; set; }

	public uint SPRecover { get; set; }

	public uint TB { get; set; }

	public uint MaxSP { get; set; }

	public uint MaxLP { get; set; }

	public uint Intel { get; set; }

	public uint CastingTime { get; set; }

	public ushort MACriRate { get; set; }

	public uint WCRateUp { get; set; }

	public uint MARateUp { get; set; }

	public ushort HpDownDamegeUp { get; set; }

	public ushort DownDamegeHp { get; set; }

	public ushort HpDownAcUp { get; set; }

	public ushort DownAcHp { get; set; }

	public ushort MoveTBUpPlus { get; set; }

	public ushort HealUPRate { get; set; }

	public ushort KeepTimeBuffUPRate { get; set; }

	public ushort CriDmgUpRate { get; set; }

	public uint ActiveSkillGroupInx { get; set; }

	public ushort DMG_MinusRate { get; set; }
}
