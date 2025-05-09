// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("SetEffect.shn")]
public class SetEffect
{
	public string SetItemIndex { get; set; }

	public byte Count { get; set; }

	public ushort ItemActionID { get; set; }
}
