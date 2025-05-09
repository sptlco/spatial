// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("Produce.shn")]
public class Produce
{
	public ushort ProductID { get; set; }

	public string ProduceIndex { get; set; }

	public string Name { get; set; }

	public string Product { get; set; }

	public uint Lot { get; set; }

	public string Raw0 { get; set; }

	public uint Quantity0 { get; set; }

	public string Raw1 { get; set; }

	public uint Quantity1 { get; set; }

	public string Raw2 { get; set; }

	public uint Quantity2 { get; set; }

	public string Raw3 { get; set; }

	public uint Quantity3 { get; set; }

	public string Raw4 { get; set; }

	public uint Quantity4 { get; set; }

	public string Raw5 { get; set; }

	public uint Quantity5 { get; set; }

	public string Raw6 { get; set; }

	public uint Quantity6 { get; set; }

	public string Raw7 { get; set; }

	public uint Quantity7 { get; set; }

	public uint MasteryType { get; set; }

	public uint MasteryGain { get; set; }

	public uint NeededMasteryType { get; set; }

	public uint NeededMasteryGain { get; set; }
}
