// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MapWayPoint.shn")]
public class MapWayPoint
{
	public ushort MapID { get; set; }

	public uint X { get; set; }

	public uint Y { get; set; }

	public byte MWP_Gate { get; set; }
}
