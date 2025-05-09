// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MobViewInfo.shn")]
public class MobViewInfo
{
	public ushort ID { get; set; }

	public string InxName { get; set; }

	public string FileName { get; set; }

	public string Texture { get; set; }

	public uint AttackType { get; set; }

	public string ShotEffect { get; set; }

	public string MobPortrait { get; set; }

	public uint ChrMarkSize { get; set; }

	public uint MiniMapIcon { get; set; }

	public ushort NpcViewIndex { get; set; }

	public ushort BoundingBox { get; set; }

	public ushort EffectViewID { get; set; }

	public byte SpectralGlow { get; set; }

	public string Group1 { get; set; }

	public string Group2 { get; set; }

	public string Group3 { get; set; }

	public string GroupS { get; set; }
}
