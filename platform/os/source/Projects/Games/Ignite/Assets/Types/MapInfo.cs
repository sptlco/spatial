// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MapInfo.shn")]
public class MapInfo
{
	public ushort ID { get; set; }

	public string MapName { get; set; }

	public string Name { get; set; }

	public uint IsWMLink { get; set; }

	public uint RegenX { get; set; }

	public uint RegenY { get; set; }

	public byte KingdomMap { get; set; }

	public string MapFolderName { get; set; }

	public byte InSide { get; set; }

	public uint Sight { get; set; }
}
