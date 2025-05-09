// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("CharacterTitleStateServer.shn")]
public class CharacterTitleStateServer
{
	public uint Type { get; set; }

	public byte TitleLV { get; set; }

	public string StateName { get; set; }

	public byte Strength { get; set; }
}
