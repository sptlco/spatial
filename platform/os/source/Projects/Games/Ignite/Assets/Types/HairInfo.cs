// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("HairInfo.shn")]
public class HairInfo
{
	public byte ID { get; set; }

	public string IndexName { get; set; }

	public string HairName { get; set; }

	public byte Grade { get; set; }

	public uint fighter { get; set; }

	public uint archer { get; set; }

	public uint cleric { get; set; }

	public uint mage { get; set; }

	public uint Joker { get; set; }

	public uint Sentinel { get; set; }

	public byte ucIsLink_Front { get; set; }

	public string acModelName_Front { get; set; }

	public string FrontTex { get; set; }

	public byte ucIsLink_Bottom { get; set; }

	public string acModelName_Bottom { get; set; }

	public string BottomTex { get; set; }

	public byte ucIsLink_Top { get; set; }

	public string acModelName_Top { get; set; }

	public string TopTex { get; set; }

	public uint Exception1 { get; set; }

	public uint Exception2 { get; set; }

	public byte ucIsLink_Acc { get; set; }

	public string acModelName_Acc { get; set; }

	public string Acc1Tex { get; set; }

	public byte ucIsLink_Acc2 { get; set; }

	public string acModelName_Acc2 { get; set; }

	public string Acc2Tex { get; set; }

	public byte ucIsLink_Acc3 { get; set; }

	public string acModelName_Acc3 { get; set; }

	public string Acc3Tex { get; set; }
}
