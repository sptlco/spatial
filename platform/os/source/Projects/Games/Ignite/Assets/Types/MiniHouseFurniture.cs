// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MiniHouseFurniture.shn")]
public class MiniHouseFurniture
{
	public ushort Handle { get; set; }

	public string ItemID { get; set; }

	public string FurnitureType { get; set; }

	public string InvenType { get; set; }

	public uint GameType { get; set; }

	public byte CanSet { get; set; }

	public string Backimage { get; set; }

	public byte WALL { get; set; }

	public byte BOTTOM { get; set; }

	public byte CEILING { get; set; }

	public byte IsAnimation { get; set; }

	public ushort Weight { get; set; }

	public ushort KeepTime_Hour { get; set; }

	public ushort KeepTime_Endure { get; set; }

	public byte Grip { get; set; }

	public byte MaxSlot { get; set; }

	public byte MHEmotionID { get; set; }
}
