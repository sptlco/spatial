// Copyright © Spatial. All rights reserved.

using Ignite.Models;
using Spatial.Networking;
using System;

namespace Ignite.Contracts.Miscellaneous;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_MISC_SERVER_TIME_NOTIFY_CMD"/>.
/// </summary>
public class PROTO_NC_MISC_SERVER_TIME_NOTIFY_CMD : ProtocolBuffer
{
    /// <summary>
    /// Create a new <see cref="PROTO_NC_MISC_SERVER_TIME_NOTIFY_CMD"/>.
    /// </summary>
    public PROTO_NC_MISC_SERVER_TIME_NOTIFY_CMD()
    {
        var time = World.Time.ToDateTime().ToLocalTime();
        
        dCurrentTM = new tm {
            tm_sec = time.Second,
            tm_min = time.Minute,
            tm_hour = time.Hour,
            tm_mday = time.Day,
            tm_mon = time.Month - 1,
            tm_year = time.Year - 1900,
            tm_wday = (int) time.DayOfWeek,
            tm_yday = time.DayOfYear - 1,
            tm_isdst = time.IsDaylightSavingTime() ? 1 : 0
        };
        
        nTimeZone = (sbyte) ((sbyte) (int) TimeZoneInfo.Local.GetUtcOffset(time).TotalSeconds / 16);
    }

    /// <summary>
    /// The current time.
    /// </summary>
    public tm dCurrentTM;

    /// <summary>
    /// The server's time zone.
    /// </summary>
    public sbyte nTimeZone;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        dCurrentTM = Read<tm>();
        nTimeZone = ReadSByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(dCurrentTM);
        Write(nTimeZone);
    }
}