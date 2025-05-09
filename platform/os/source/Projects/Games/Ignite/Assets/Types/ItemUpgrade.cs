// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ItemUpgrade.shn")]
public class ItemUpgrade
{
	public uint ID { get; set; }

	public ushort CriFail { get; set; }

	public ushort DownFail { get; set; }

	public ushort NormalFail { get; set; }

	public ushort nCon { get; set; }

	public ushort LuckySuc { get; set; }
}
