// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("GradeItemOption.shn")]
public class GradeItemOption
{
	public string ItemIndex { get; set; }

	public ushort STR { get; set; }

	public ushort CON { get; set; }

	public ushort DEX { get; set; }

	public ushort INT { get; set; }

	public ushort MEN { get; set; }

	public ushort ResistPoison { get; set; }

	public ushort ResistDeaseas { get; set; }

	public ushort ResistCurse { get; set; }

	public ushort ResistMoveSpdDown { get; set; }

	public ushort ToHitRate { get; set; }

	public ushort ToBlockRate { get; set; }

	public ushort MaxHP { get; set; }

	public ushort MaxSP { get; set; }

	public ushort WCPlus { get; set; }

	public ushort MAPlus { get; set; }
}
