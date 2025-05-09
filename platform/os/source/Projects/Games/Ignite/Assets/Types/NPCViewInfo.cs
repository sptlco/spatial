// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("NPCViewInfo.shn")]
public class NPCViewInfo
{
	public ushort TypeIndex { get; set; }

	public uint Class { get; set; }

	public ushort Gender { get; set; }

	public byte FaceShape { get; set; }

	public byte HairType { get; set; }

	public byte HairColor { get; set; }

	public uint BaseActionCode { get; set; }

	public uint PeriodActionCode { get; set; }

	public uint ActionDelayTime { get; set; }

	public byte bUseEventAction { get; set; }

	public string Equ_RightHand { get; set; }

	public string Equ_LeftHand { get; set; }

	public string Equ_Body { get; set; }

	public string Equ_Leg { get; set; }

	public string Equ_Shoes { get; set; }

	public string Equ_AccBody { get; set; }

	public string Equ_AccLeg { get; set; }

	public string Equ_AccShoes { get; set; }

	public string Equ_AccMouth { get; set; }

	public string Equ_AccHeadA { get; set; }

	public string Equ_AccEye { get; set; }

	public string Equ_AccHead { get; set; }

	public string Equ_AccLeftHand { get; set; }

	public string Equ_AccRightHand { get; set; }

	public string Equ_AccBack { get; set; }

	public string Equ_AccWeast { get; set; }

	public string Equ_AccHip { get; set; }

	public string Equ_MiniMon { get; set; }

	public string Equ_MiniMon_R { get; set; }
}
