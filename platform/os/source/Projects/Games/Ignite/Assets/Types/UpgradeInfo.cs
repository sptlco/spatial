// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("UpgradeInfo.shn")]
public class UpgradeInfo
{
	public ushort ID { get; set; }

	public string InxName { get; set; }

	public uint UpFactor { get; set; }

	public ushort[] Updata { get; set; }
}
