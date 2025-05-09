// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("AbState.shn")]
public class AbState
{
	public ushort ID { get; set; }

	public string InxName { get; set; }

	public uint AbStataIndex { get; set; }

	public uint KeepTimeRatio { get; set; }

	public byte KeepTimePower { get; set; }

	public byte StateGrade { get; set; }

	public string PartyState1 { get; set; }

	public string PartyState2 { get; set; }

	public string PartyState3 { get; set; }

	public string PartyState4 { get; set; }

	public string PartyState5 { get; set; }

	public uint PartyRange { get; set; }

	public uint PartyEnchantNumber { get; set; }

	public string SubAbState { get; set; }

	public uint DispelIndex { get; set; }

	public uint SubDispelIndex { get; set; }

	public uint AbStateSaveType { get; set; }

	public string MainStateInx { get; set; }

	public byte Duplicate { get; set; }
}
