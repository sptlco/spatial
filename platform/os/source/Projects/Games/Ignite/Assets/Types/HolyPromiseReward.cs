// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("HolyPromiseReward.shn")]
public class HolyPromiseReward
{
	public byte Level { get; set; }

	public byte Class { get; set; }

	public string ItemIndex { get; set; }

	public ushort Lot { get; set; }

	public byte Upgrade { get; set; }

	public uint IO_Str { get; set; }

	public uint IO_Con { get; set; }

	public uint IO_Dex { get; set; }

	public uint IO_Int { get; set; }

	public uint IO_Men { get; set; }

	public uint Res1 { get; set; }

	public uint Res2 { get; set; }

	public uint Res3 { get; set; }
}
