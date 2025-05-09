// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ActionViewInfo.shn")]
public class ActionViewInfo
{
	public byte nIndex { get; set; }

	public string InxName { get; set; }

	public string ActionName { get; set; }

	public ushort LinkActionIndex { get; set; }

	public string IconFileName { get; set; }

	public ushort nIconNum { get; set; }

	public uint eActionType { get; set; }

	public uint nEventCode { get; set; }

	public uint nAfterCode { get; set; }

	public byte IsDance { get; set; }
}
