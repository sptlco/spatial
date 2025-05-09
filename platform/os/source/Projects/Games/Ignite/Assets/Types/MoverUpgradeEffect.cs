// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MoverUpgradeEffect.shn")]
public class MoverUpgradeEffect
{
	public ushort RunSpeed { get; set; }

	public ushort HPSPRecoveryTick { get; set; }

	public ushort HPSPRecovery { get; set; }

	public ushort CastingTime { get; set; }

	public ushort CastingCoolTime { get; set; }

	public string EffectFileName { get; set; }

	public string AbStateIDX { get; set; }

	public byte Strength { get; set; }
}
