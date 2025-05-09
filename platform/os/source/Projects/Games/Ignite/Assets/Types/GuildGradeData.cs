// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("GuildGradeData.shn")]
public class GuildGradeData
{
	public byte Type { get; set; }

	public uint NeedFame { get; set; }

	public ushort MaxOfMember { get; set; }

	public ushort[] MaxOfGradeMember { get; set; }
}
