// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MobAutoAction.shn")]
public class MobAutoAction
{
	public string MobInx { get; set; }

	public uint Attack { get; set; }

	public uint Target { get; set; }

	public uint ActionType { get; set; }

	public string StateInx { get; set; }

	public byte Strength { get; set; }

	public ushort EffectRate { get; set; }

	public ushort Range { get; set; }
}
