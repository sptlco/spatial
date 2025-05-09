// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("KQTeam.shn")]
public class KQTeam
{
	public ushort ID { get; set; }

	public byte MaxMemberGap { get; set; }

	public byte IsTeamPVP { get; set; }

	public ushort KQTeamDivideType { get; set; }

	public uint RegenXRed { get; set; }

	public uint RegenYRed { get; set; }

	public uint RegenXBlue { get; set; }

	public uint RegenYBlue { get; set; }
}
