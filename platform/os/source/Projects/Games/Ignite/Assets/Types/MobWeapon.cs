// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MobWeapon.shn")]
public class MobWeapon
{
	public uint ID { get; set; }

	public string InxName { get; set; }

	public string Skill { get; set; }

	public ushort AtkSpd { get; set; }

	public ushort BlastRate { get; set; }

	public ushort AtkDly { get; set; }

	public ushort SwingTime { get; set; }

	public ushort HitTime { get; set; }

	public uint AtkType { get; set; }

	public uint MinWC { get; set; }

	public uint MaxWC { get; set; }

	public ushort TH { get; set; }

	public uint MinMA { get; set; }

	public uint MaxMA { get; set; }

	public ushort MH { get; set; }

	public ushort Range { get; set; }

	public uint MopAttackTarget { get; set; }

	public uint HitType { get; set; }

	public string StaName { get; set; }

	public ushort StaStrength { get; set; }

	public ushort StaRate { get; set; }

	public ushort AggroInitialize { get; set; }
}
