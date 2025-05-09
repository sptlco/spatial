// Copyright © Spatial. All rights reserved.

using System.Collections.Generic;

namespace Ignite.Assets.Types;

[Name("ItemActionCondition.shn")]
public class ItemActionCondition
{
	public ushort ConditionID { get; set; }

	public KeyValuePair<uint, uint> SubjectTarget { get; set; }

	public KeyValuePair<uint, uint> ObjectTarget { get; set; }

	public KeyValuePair<uint, uint> ConditionActivity { get; set; }

	public ushort ActivityRate { get; set; }

	public ushort Range { get; set; }
}
