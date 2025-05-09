// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing temporal data.
/// </summary>
public class tm : ProtocolBuffer
{
    /// <summary>
    /// The current second.
    /// </summary>
    public int tm_sec;

    /// <summary>
    /// The current minute.
    /// </summary>
    public int tm_min;

    /// <summary>
    /// The current hour.
    /// </summary>
    public int tm_hour;

    /// <summary>
    /// The current day of the month.
    /// </summary>
    public int tm_mday;

    /// <summary>
    /// The current month.
    /// </summary>
    public int tm_mon;

    /// <summary>
    /// The current year.
    /// </summary>
    public int tm_year;

    /// <summary>
    /// The current day of the week.
    /// </summary>
    public int tm_wday;

    /// <summary>
    /// The current day of the year.
    /// </summary>
    public int tm_yday;

    /// <summary>
    /// Whether or not daylight savings time is being observed.
    /// </summary>
    public int tm_isdst;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        tm_sec = ReadInt32();
        tm_min = ReadInt32();
        tm_hour = ReadInt32();
        tm_mday = ReadInt32();
        tm_mon = ReadInt32();
        tm_year = ReadInt32();
        tm_wday = ReadInt32();
        tm_yday = ReadInt32();
        tm_isdst = ReadInt32();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(tm_sec);
        Write(tm_min);
        Write(tm_hour);
        Write(tm_mday);
        Write(tm_mon);
        Write(tm_year);
        Write(tm_wday);
        Write(tm_yday);
        Write(tm_isdst);
    }
}