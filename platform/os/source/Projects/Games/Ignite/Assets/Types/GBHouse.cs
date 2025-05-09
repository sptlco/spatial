// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("GBHouse.shn")]
public class GBHouse
{
	public uint GB_GameMoney { get; set; }

	public uint GB_ExchangeTax { get; set; }

	public byte GB_ResetTimeHour { get; set; }

	public byte GB_ResetTimeMin { get; set; }

	public byte GB_ResetTimeSec { get; set; }
}
