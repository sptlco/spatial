// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MiniHouseFurnitureObjEffect.shn")]
public class MiniHouseFurnitureObjEffect
{
	public ushort Handle { get; set; }

	public string ItemID { get; set; }

	public uint EffectEnum { get; set; }

	public string EffectIndex { get; set; }

	public uint[] ApplyRange { get; set; }

	public uint[] UseRange { get; set; }

	public string NeedItem { get; set; }

	public uint NeedMoney { get; set; }

	public string EffectName { get; set; }

	public string EffectSound { get; set; }
}
