// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MoverAbility.shn")]
public class MoverAbility
{
	public string MoverIDX { get; set; }

	public byte MoverLv { get; set; }

	public uint HP { get; set; }

	public uint WCMin { get; set; }

	public uint WCMax { get; set; }

	public uint MAMin { get; set; }

	public uint MAMax { get; set; }

	public ushort AC { get; set; }

	public ushort MR { get; set; }

	public ushort TH { get; set; }

	public ushort TB { get; set; }

	public string ResIndex { get; set; }

	public uint AbsoluteSize { get; set; }
}
