// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ItemOptions.shn")]
public class ItemOptions
{
	public ushort OptionDegree { get; set; }

	public uint Type { get; set; }

	public byte Interval { get; set; }

	public ushort[] Rate { get; set; }
}
