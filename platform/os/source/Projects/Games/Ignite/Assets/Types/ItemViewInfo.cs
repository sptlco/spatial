// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ItemViewInfo.shn")]
public class ItemViewInfo
{
	public ushort ID { get; set; }

	public string InxName { get; set; }

	public uint IconIndex { get; set; }

	public string IconFile { get; set; }

	public uint SubIconIndex { get; set; }

	public string SubIconFile { get; set; }

	public uint PeriodIconIndex { get; set; }

	public string PeriodIconFile { get; set; }

	public uint R { get; set; }

	public uint G { get; set; }

	public uint B { get; set; }

	public uint SUB_R { get; set; }

	public uint SUB_G { get; set; }

	public uint SUB_B { get; set; }

	public uint EquipType { get; set; }

	public string LinkFile { get; set; }

	public string TextureFile { get; set; }

	public uint MSetNo { get; set; }

	public uint FSetNo { get; set; }

	public float GrnItemSize { get; set; }

	public string GrnItemTex { get; set; }

	public ushort UpEffect { get; set; }

	public string DropSnd { get; set; }

	public string EquSnd { get; set; }

	public string PutSnd { get; set; }

	public uint IVET_Index { get; set; }

	public string Descript { get; set; }
}
