// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("WeaponAttrib.shn")]
public class WeaponAttrib
{
	public uint WeaponType { get; set; }

	public ushort UsableDegree { get; set; }

	public byte IsUsableInMoving { get; set; }

	public ushort[] HitRate { get; set; }
}
