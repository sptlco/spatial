// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MultiHitType.shn")]
public class MultiHitType
{
	public ushort ID { get; set; }

	public ushort HitTime { get; set; }

	public string AbIndex { get; set; }

	public byte AS_Step { get; set; }

	public byte AbStr { get; set; }

	public ushort AbRate { get; set; }

	public ushort DmgRate { get; set; }
}
