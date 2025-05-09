// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("AbStateSaveTypeInfo.shn")]
public class AbStateSaveTypeInfo
{
	public uint AbStateSaveType { get; set; }

	public byte IsSaveLink { get; set; }

	public byte IsSaveDie { get; set; }

	public byte IsSaveLogoff { get; set; }
}
