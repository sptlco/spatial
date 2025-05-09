// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MapLinkPoint.shn")]
public class MapLinkPoint
{
	public uint MLP_FromID { get; set; }

	public uint MLP_ToID { get; set; }

	public uint MLP_Weight { get; set; }

	public ushort MLP_OneWay_Street { get; set; }
}
