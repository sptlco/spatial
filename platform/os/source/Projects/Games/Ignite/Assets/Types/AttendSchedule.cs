// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("AttendSchedule.shn")]
public class AttendSchedule
{
	public ushort AS_StartYear { get; set; }

	public byte AS_StartMonth { get; set; }

	public byte AS_StartDay { get; set; }

	public byte AS_StartHour { get; set; }

	public byte AS_StartMinute { get; set; }

	public ushort AS_JoinKeepTime { get; set; }

	public ushort AS_CheckTerm { get; set; }
}
