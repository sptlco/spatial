// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ItemShopView.shn")]
public class ItemShopView
{
	public uint goodsNo { get; set; }

	public string Name { get; set; }

	public uint IconIndex { get; set; }

	public string IconFile { get; set; }

	public uint SubIconIndex { get; set; }

	public string SubIconFile { get; set; }

	public uint PeriodIconIndex { get; set; }

	public string PeriodIconFile { get; set; }

	public uint R { get; set; }

	public uint G { get; set; }

	public uint B { get; set; }

	public string Descript { get; set; }
}
