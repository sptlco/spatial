// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("PSkillSetAbstate.shn")]
public class PSkillSetAbstate
{
	public string PS_InxName { get; set; }

	public uint PS_Condition { get; set; }

	public ushort PS_ConditioRate { get; set; }

	public string PS_AbStateInx { get; set; }

	public byte Strength { get; set; }
}
