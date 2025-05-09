// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("EffectViewInfo.shn")]
public class EffectViewInfo
{
	public ushort ID { get; set; }

	public uint Amplitude { get; set; }

	public uint Period { get; set; }

	public uint Damp { get; set; }

	public uint Axis { get; set; }
}
