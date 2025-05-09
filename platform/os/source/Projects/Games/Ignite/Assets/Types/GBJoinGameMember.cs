// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("GBJoinGameMember.shn")]
public class GBJoinGameMember
{
	public uint GameType { get; set; }

	public byte MinJoinMember { get; set; }

	public byte MaxJoinMember { get; set; }
}
