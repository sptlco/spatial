// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ItemDismantle.shn")]
public class ItemDismantle
{
	public byte ID { get; set; }

	public byte Grade { get; set; }

	public uint[] Armor { get; set; }

	public uint[] Boot { get; set; }

	public uint[] Shield { get; set; }

	public uint[] Weapon { get; set; }

	public uint[] Amulet { get; set; }
}
