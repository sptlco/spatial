// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ChargedEffect.shn")]
public class ChargedEffect
{
	public ushort Handle { get; set; }

	public string ItemID { get; set; }

	public ushort KeepTime_Hour { get; set; }

	public uint EffectEnum { get; set; }

	public ushort EffectValue { get; set; }

	public byte StaStrength { get; set; }
}
