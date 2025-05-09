// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("PupFactorCondition.shn")]
public class PupFactorCondition
{
	public uint PupMindType { get; set; }

	public uint PupFactorConditionType { get; set; }

	public uint PupFactorType { get; set; }

	public byte IsMinus { get; set; }

	public uint Value { get; set; }
}
