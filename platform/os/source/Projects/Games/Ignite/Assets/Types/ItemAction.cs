// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("ItemAction.shn")]
public class ItemAction
{
	public ushort ItemActionID { get; set; }

	public ushort ConditionID { get; set; }

	public ushort EffectID { get; set; }

	public uint CoolTime { get; set; }

	public ushort CoolGroupActionID { get; set; }
}
