// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MobAbStateDropSetting.shn")]
public class MobAbStateDropSetting
{
	public string MobInx { get; set; }

	public string ABStateInx { get; set; }

	public uint DropType { get; set; }

	public byte MaxCount { get; set; }
}
