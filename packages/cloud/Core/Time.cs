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
    public static Time Now => FromMilliseconds(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

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
    public double Ticks => Milliseconds * TimeSpan.TicksPerMillisecond;

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
        if (time.Kind == DateTimeKind.Unspecified)
        {
            time = DateTime.SpecifyKind(time, DateTimeKind.Utc);
        }

        return FromMilliseconds(new DateTimeOffset(time).ToUnixTimeMilliseconds());
    }

    /// <summary>
    /// Convert the <see cref="Time"/> to a <see cref="DateTime"/>.
    /// </summary>
    /// <returns>A <see cref="DateTime"/>.</returns>
    public DateTime ToDateTime()
    {
         return DateTimeOffset.FromUnixTimeMilliseconds((long)_milliseconds).UtcDateTime;
    }

    /// <summary>
    /// Get the <see cref="Time"/> as a <see cref="TimeSpan"/>.
    /// </summary>
    /// <returns>A <see cref="TimeSpan"/>.</returns>
    public TimeSpan AsTimeSpan()
    {
        return TimeSpan.FromMilliseconds(_milliseconds);
    }

    /// <summary>
    /// Decompose this <see cref="Time"/> into its calendar components in a single pass.
    /// </summary>
    /// <returns>A tuple containing the year, month, day, hour, minute, and second.</returns>
    public (int Year, int Month, int Day, int Hour, int Minute, int Second) Decompose()
    {
        var totalSeconds = (long) _milliseconds / 1000;
        var days = Floor(totalSeconds, 86400);
        var secOfDay = totalSeconds - days * 86400;

        var hour = (int) (secOfDay / 3600);
        var minute = (int) ((secOfDay % 3600) / 60);
        var second = (int) (secOfDay % 60);

        var (year, month, day) = ToCivil(days);

        return (year, month, day, hour, minute, second);
    }

    private static (int Year, int Month, int Day) ToCivil(long z)
    {
        // Days since 1970-01-01 -> (year, month, day). Proleptic Gregorian calendar.
        // See: Howard Hinnant's "civil_from_days" algorithm.

        z += 719468; // shift epoch to 0000-03-01

        var era = Floor(z >= 0 ? z : z - 146096, 146097);
        var doe = z - era * 146097;                                    // [0, 146096]
        var yoe = (doe - doe / 1460 + doe / 36524 - doe / 146096) / 365; // [0, 399]
        var y = yoe + era * 400;
        var doy = doe - (365 * yoe + yoe / 4 - yoe / 100);             // [0, 365]
        var mp = (5 * doy + 2) / 153;                                  // [0, 11]
        var d = doy - (153 * mp + 2) / 5 + 1;                          // [1, 31]
        var m = mp + (mp < 10 ? 3 : -9);                               // [1, 12]

        y += m <= 2 ? 1 : 0;

        return ((int) y, (int) m, (int) d);
    }

    private static long Floor(long a, long b)
    {
        long q = a / b;
        if (a % b != 0 && (a < 0) != (b < 0)) q--;
        return q;
    }
}