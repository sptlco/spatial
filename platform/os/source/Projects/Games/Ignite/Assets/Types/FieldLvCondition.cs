// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("FieldLvCondition.shn")]
public class FieldLvCondition
{
	public string MapIndex { get; set; }

	public byte ModeIDLv { get; set; }

	public ushort LvFrom { get; set; }

	public ushort LvTo { get; set; }
}
