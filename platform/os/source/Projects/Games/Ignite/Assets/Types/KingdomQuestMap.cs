// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("KingdomQuestMap.shn")]
public class KingdomQuestMap
{
	public byte NumOfMap { get; set; }

	public string BaseMap { get; set; }

	public string[] Map { get; set; }

	public byte[] Clear { get; set; }
}
