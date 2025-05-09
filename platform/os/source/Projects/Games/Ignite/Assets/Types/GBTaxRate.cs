// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("GBTaxRate.shn")]
public class GBTaxRate
{
	public uint GameType { get; set; }

	public ushort GB_TaxRate { get; set; }

	public uint GB_JPSave { get; set; }

	public ushort GB_JPSaveRate { get; set; }
}
