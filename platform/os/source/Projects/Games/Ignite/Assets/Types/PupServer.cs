// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("PupServer.shn")]
public class PupServer
{
	public string PupIDX { get; set; }

	public byte MinMind { get; set; }

	public byte MaxMind { get; set; }

	public byte MinStress { get; set; }

	public byte MaxStress { get; set; }
}
