// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

/// <summary>
/// Represents a precise duration or timestamp, stored internally in milliseconds.
/// </summary>
public readonly struct Time
{
    /// <summary>
    /// A zero-length <see cref="Time"/> duration.
    /// </summary>
    public static readonly Time Zero = new(0);

    /// <summary>
    /// The current <see cref="Time"/>.
    /// </summary>
    public static Time Now => FromDateTime(DateTime.UtcNow);

    private readonly double _milliseconds;

    /// <summary>
    /// Create a new <see cref="Time"/>.
    /// </summary>
    /// <param name="milliseconds">An amount of milliseconds.</param>
    public Time(double milliseconds)
    {
        _milliseconds = milliseconds;
    }

    /// <summary>
    /// The <see cref="Time"/> in milliseconds.
    /// </summary>
    public double Milliseconds => _milliseconds;

    /// <summary>
    /// The <see cref="Time"/> in seconds.
    /// </summary>
    public double Seconds => Milliseconds / 1000D;

    /// <summary>
    /// The current tick count.
    /// </summary>
    public long Ticks => (long) (Milliseconds * TimeSpan.TicksPerMillisecond);

    /// <summary>
    /// Convert a <see cref="Time"/> to milliseconds.
    /// </summary>
    /// <param name="time">A <see cref="Time"/> value.</param>
    public static implicit operator double(Time time) => time._milliseconds;

    /// <summary>
    /// Convert milliseconds to <see cref="Time"/>.
    /// </summary>
    /// <param name="milliseconds">An amount of milliseconds.</param>
    public static implicit operator Time(double milliseconds) => new(milliseconds);

    /// <summary>
    /// Create a new <see cref="Time"/> from a number of milliseconds.
    /// </summary>
    /// <param name="milliseconds">A number of milliseconds.</param>
    /// <returns>A <see cref="Time"/>.</returns>
    public static Time FromMilliseconds(double milliseconds) => new(milliseconds);

    /// <summary>
    /// Create a new <see cref="Time"/> from a number of seconds.
    /// </summary>
    /// <param name="seconds">A number of seconds.</param>
    /// <returns>A <see cref="Time"/>.</returns>
    public static Time FromSeconds(double seconds) => FromMilliseconds(seconds * 1000D);

    /// <summary>
    /// Create a new <see cref="Time"/> from a number of minutes.
    /// </summary>
    /// <param name="minutes">A number of minutes.</param>
    /// <returns>A <see cref="Time"/>.</returns>
    public static Time FromMinutes(double minutes) => FromSeconds(minutes * 60D);

    /// <summary>
    /// Create a new <see cref="Time"/> from a number of hours.
    /// </summary>
    /// <param name="hours">A number of hours.</param>
    /// <returns>A <see cref="Time"/>.</returns>
    public static Time FromHours(double hours) => FromMinutes(hours * 60);

    /// <summary>
    /// Create a new <see cref="Time"/> from a number of days.
    /// </summary>
    /// <param name="days">A number of days.</param>
    /// <returns>A <see cref="Time"/>.</returns>
    public static Time FromDays(double days) => FromHours(days * 24.0D);

    /// <summary>
    /// Create a new <see cref="Time"/> from a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="time">A <see cref="DateTime"/>.</param>
    /// <returns>A <see cref="Time"/>.</returns>
    public static Time FromDateTime(DateTime time)
    {
        return new(time.Ticks / (double) TimeSpan.TicksPerMillisecond);
    }

    /// <summary>
    /// Convert the <see cref="Time"/> to a <see cref="DateTime"/>.
    /// </summary>
    /// <returns>A <see cref="DateTime"/>.</returns>
    public DateTime ToDateTime()
    {
        return new DateTime((long) (_milliseconds * TimeSpan.TicksPerMillisecond), DateTimeKind.Utc);
    }

    /// <summary>
    /// Get the <see cref="Time"/> as a <see cref="TimeSpan"/>.
    /// </summary>
    /// <returns>A <see cref="TimeSpan"/>.</returns>
    public TimeSpan AsTimeSpan()
    {
        return TimeSpan.FromMilliseconds(_milliseconds);
    }
}