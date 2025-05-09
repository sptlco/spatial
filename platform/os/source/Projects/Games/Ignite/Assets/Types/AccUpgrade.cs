// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("AccUpgrade.shn")]
public class AccUpgrade
{
	public uint ID { get; set; }

	public ushort CriFail { get; set; }

	public ushort DownFail { get; set; }

	public ushort NormalFail { get; set; }

	public ushort nCon { get; set; }

	public ushort LuckySuc { get; set; }
}
