// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("SetItemEffect.shn")]
public class SetItemEffect
{
	public string Effect { get; set; }

	public string Desc { get; set; }

	public byte UseSubject { get; set; }

	public string SkillGroup { get; set; }

	public string From { get; set; }

	public string To { get; set; }

	public uint Index { get; set; }

	public uint Argument { get; set; }
}
