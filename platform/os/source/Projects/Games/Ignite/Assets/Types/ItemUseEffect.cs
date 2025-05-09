// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ItemUseEffect.shn")]
public class ItemUseEffect
{
	public string ItemIndex { get; set; }

	public uint UseEffectA { get; set; }

	public ushort UseValueA { get; set; }

	public uint UseEffectB { get; set; }

	public ushort UseValueB { get; set; }

	public uint UseEffectC { get; set; }

	public ushort UseValueC { get; set; }

	public string UseAbstateName { get; set; }

	public uint UseAbstateIndex { get; set; }
}
