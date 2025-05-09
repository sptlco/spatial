// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MapViewInfo.shn")]
public class MapViewInfo
{
	public ushort ID { get; set; }

	public string MapName { get; set; }

	public string BGM_Name_01 { get; set; }

	public string BGM_Name_02 { get; set; }

	public string BGM_Name_03 { get; set; }

	public ushort BGMDlyTime { get; set; }

	public float MiniMapScale { get; set; }

	public byte Enlargement { get; set; }

	public byte KingdomMap { get; set; }

	public string MapFolderName { get; set; }

	public byte MinimapView { get; set; }

	public byte WorldMapView { get; set; }

	public string Loading { get; set; }

	public uint BGMVol { get; set; }

	public byte LoadLocation { get; set; }

	public ushort StartX { get; set; }

	public ushort StartY { get; set; }

	public ushort EndX { get; set; }

	public ushort EndY { get; set; }

	public string WeatherEffect { get; set; }

	public ushort ZoomMax { get; set; }

	public byte MiniMapSort { get; set; }

	public uint MWP_IsMove { get; set; }
}
