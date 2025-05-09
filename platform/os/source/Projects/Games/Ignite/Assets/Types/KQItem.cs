// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("KQItem.shn")]
public class KQItem
{
	public string ItemIndex { get; set; }

	public ushort MoveSpdRate { get; set; }

	public ushort AbsoluteAttack { get; set; }

	public ushort PickupLimit { get; set; }
}
