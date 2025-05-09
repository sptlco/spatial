// Copyright © Spatial. All rights reserved.

using System.Collections.Generic;

namespace Ignite.Assets.Types;

[Name("ItemActionEffect.shn")]
public class ItemActionEffect
{
	public ushort EffectID { get; set; }

	public KeyValuePair<uint, uint> EffectTarget { get; set; }

	public KeyValuePair<uint, uint> EffectActivity { get; set; }

	public ushort Value { get; set; }

	public ushort Area { get; set; }
}
