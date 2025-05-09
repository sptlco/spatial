// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ItemPackage.shn")]
public class ItemPackage
{
	public ushort Handle { get; set; }

	public string ItemID { get; set; }

	public string Content { get; set; }

	public ushort Number { get; set; }
}
