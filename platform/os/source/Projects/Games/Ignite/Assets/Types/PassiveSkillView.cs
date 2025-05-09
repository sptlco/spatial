// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("PassiveSkillView.shn")]
public class PassiveSkillView
{
	public ushort ID { get; set; }

	public string InxName { get; set; }

	public uint Icon { get; set; }

	public string IconFile { get; set; }

	public uint R { get; set; }

	public uint G { get; set; }

	public uint B { get; set; }

	public string Descript { get; set; }

	public uint DemandLv { get; set; }

	public uint UseClass { get; set; }
}
