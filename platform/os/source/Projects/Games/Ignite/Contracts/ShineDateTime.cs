// Copyright © Spatial. All rights reserved.

using Spatial.Networking;
using Spatial.Simulation;
using System;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing time data.
/// </summary>
public class ShineDateTime : ProtocolBuffer
{
    /// <summary>
    /// The maximum value for a <see cref="ShineDateTime"/>.
    /// </summary>
    public static ShineDateTime MaxValue => new ShineDateTime {
        year = 2255,
        month = 12,
        day = 31,
        hour = 23,
        min = 59
    };

    /// <summary>
    /// The time's year.
    /// </summary>
    public uint year;

    /// <summary>
    /// The time's month.
    /// </summary>
    public uint month;

    /// <summary>
    /// The time's day.
    /// </summary>
    public uint day;

    /// <summary>
    /// The time's hour.
    /// </summary>
    public uint hour;

    /// <summary>
    /// The time's minute.
    /// </summary>
    public uint min;

    /// <summary>
    /// Create a new <see cref="ShineDateTime"/>.
    /// </summary>
    public ShineDateTime() { }

    /// <summary>
    /// Create a new <see cref="ShineDateTime"/>.
    /// </summary>
    /// <param name="time">A <see cref="Time"/>.</param>
    public ShineDateTime(Time time) : this(time.ToDateTime()) { }

    /// <summary>
    /// Create a new <see cref="ShineDateTime"/>.
    /// </summary>
    /// <param name="time">A <see cref="DateTime"/>.</param>
    public ShineDateTime(DateTime time)
    {
        year = (uint) time.Year;
        month = (uint) time.Month;
        day = (uint) time.Day;
        hour = (uint) time.Hour;
        min = (uint) time.Minute;
    }

    /// <summary>
    /// Convert the <see cref="ShineDateTime"/> to a <see cref="uint"/>.
    /// </summary>
    /// <param name="time">A <see cref="ShineDateTime"/>.</param>
    public static implicit operator uint(ShineDateTime time) => time.GetTimestamp();

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        var timestamp = ReadUInt32();

        year = (uint) ((byte) timestamp + 2000);
        month = (timestamp >> 8) & 0x1F;
        day = (timestamp >> 13) & 0x3F;
        hour = (timestamp >> 19) & 0x3F;
        min = timestamp >> 25;
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(GetTimestamp());
    }

    private uint GetTimestamp()
    {
        uint timestamp = 0;

        timestamp |= (year - 2000) & 0xFF;
        timestamp |= (month & 0x1F) << 8;
        timestamp |= (day & 0x3F) << 13;
        timestamp |= (hour & 0x3F) << 19;
        timestamp |= (min & 0x7F) << 25;

        return timestamp;
    }
}
