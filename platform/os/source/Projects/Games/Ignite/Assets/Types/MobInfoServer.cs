// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;

namespace Ignite.Assets.Types;

[Name("MobInfoServer.shn")]
public class MobInfoServer
{
	public uint ID { get; set; }

	public string InxName { get; set; }

	public byte Visible { get; set; }

	public ushort AC { get; set; }

	public ushort TB { get; set; }

	public ushort MR { get; set; }

	public ushort MB { get; set; }

	public EnemyDetect EnemyDetectType { get; set; }

	public uint MobKillInx { get; set; }

	public uint MonEXP { get; set; }

	public ushort EXPRange { get; set; }

	public ushort DetectCha { get; set; }

	public byte ResetInterval { get; set; }

	public ushort CutInterval { get; set; }

	public uint CutNonAT { get; set; }

	public uint FollowCha { get; set; }

	public ushort PceHPRcvDly { get; set; }

	public ushort PceHPRcv { get; set; }

	public ushort AtkHPRcvDly { get; set; }

	public ushort AtkHPRcv { get; set; }

	public ushort Str { get; set; }

	public ushort Dex { get; set; }

	public ushort Con { get; set; }

	public ushort Int { get; set; }

	public ushort Men { get; set; }

	public uint MobRaceType { get; set; }

	public byte Rank { get; set; }

	public uint FamilyArea { get; set; }

	public uint FamilyRescArea { get; set; }

	public byte FamilyRescCount { get; set; }

	public ushort BloodingResi { get; set; }

	public ushort StunResi { get; set; }

	public ushort MoveSpeedResi { get; set; }

	public ushort FearResi { get; set; }

	public string ResIndex { get; set; }

	public ushort KQKillPoint { get; set; }

	public byte Return2Regen { get; set; }

	public byte IsRoaming { get; set; }

	public byte RoamingNumber { get; set; }

	public ushort RoamingDistance { get; set; }

	public ushort RoamingRestTime { get; set; }

	public ushort MaxSP { get; set; }

	public byte BroadAtDead { get; set; }

	public ushort TurnSpeed { get; set; }

	public ushort WalkChase { get; set; }

	public byte AllCanLoot { get; set; }

	public ushort DmgByHealMin { get; set; }

	public ushort DmgByHealMax { get; set; }

	public ushort RegenInterval { get; set; }
}
