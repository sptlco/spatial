// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("SubAbState.shn")]
public class SubAbState
{
	public ushort ID { get; set; }

	public string InxName { get; set; }

	public uint Strength { get; set; }

	public uint Type { get; set; }

	public byte SubType { get; set; }

	public uint KeepTime { get; set; }

	public uint ActionIndexA { get; set; }

	public uint ActionArgA { get; set; }

	public uint ActionIndexB { get; set; }

	public uint ActionArgB { get; set; }

	public uint ActionIndexC { get; set; }

	public uint ActionArgC { get; set; }

	public uint ActionIndexD { get; set; }

	public uint ActionArgD { get; set; }
}
