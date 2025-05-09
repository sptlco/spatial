// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ProduceView.shn")]
public class ProduceView
{
	public ushort ID { get; set; }

	public string InxName { get; set; }

	public string Name { get; set; }

	public ushort IconIndex { get; set; }

	public string IconFile { get; set; }

	public ushort SubIconIndex { get; set; }

	public string SubIconFile { get; set; }

	public uint MasteryType { get; set; }

	public uint RowMasteryGain { get; set; }

	public uint NorMasteryGain { get; set; }

	public uint HighMasteryGain { get; set; }

	public uint BestMasteryGain { get; set; }
}
