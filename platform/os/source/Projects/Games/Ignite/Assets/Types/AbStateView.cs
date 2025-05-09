// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("AbStateView.shn")]
public class AbStateView
{
	public ushort ID { get; set; }

	public string inxName { get; set; }

	public uint icon { get; set; }

	public string iconFile { get; set; }

	public string Descript { get; set; }

	public byte R { get; set; }

	public byte G { get; set; }

	public byte B { get; set; }

	public string AniIndex { get; set; }

	public string effName { get; set; }

	public uint EffNamePos { get; set; }

	public byte EffRefresh { get; set; }

	public string LoopEffect { get; set; }

	public uint LoopEffPos { get; set; }

	public string LastEffect { get; set; }

	public uint LastEffectPos { get; set; }

	public string DOTEffect { get; set; }

	public uint DOTEffectPos { get; set; }

	public string IconSort { get; set; }

	public uint TypeSort { get; set; }

	public byte View { get; set; }
}
