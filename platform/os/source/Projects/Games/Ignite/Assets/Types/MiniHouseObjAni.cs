// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MiniHouseObjAni.shn")]
public class MiniHouseObjAni
{
	public ushort Handle { get; set; }

	public ushort ItemID { get; set; }

	public byte AniGroupIDMaxNum { get; set; }

	public ushort AniGroupID { get; set; }

	public uint EventCode { get; set; }

	public ushort NextAniHandle { get; set; }

	public byte ActorMaxNum { get; set; }

	public uint Actor01 { get; set; }

	public uint Actor02 { get; set; }

	public uint Actor03 { get; set; }

	public byte ActeeMaxNum { get; set; }

	public uint Actee01 { get; set; }

	public uint Actee02 { get; set; }

	public uint Actee03 { get; set; }
}
