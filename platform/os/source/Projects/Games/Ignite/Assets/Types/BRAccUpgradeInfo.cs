// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("BRAccUpgradeInfo.shn")]
public class BRAccUpgradeInfo
{
	public ushort ID { get; set; }

	public string InxName { get; set; }

	public uint UpFactor { get; set; }

	public ushort[] BRAccUpdata { get; set; }
}
