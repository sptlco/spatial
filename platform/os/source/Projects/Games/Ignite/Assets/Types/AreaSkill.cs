// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("AreaSkill.shn")]
public class AreaSkill
{
	public string AS_SkillInx { get; set; }

	public byte AS_Step { get; set; }

	public string AS_BMPIndex { get; set; }

	public uint AS_ImagePin { get; set; }

	public byte AS_IsDirection { get; set; }
}
