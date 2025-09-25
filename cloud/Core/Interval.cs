// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Caching;

namespace Spatial;

/// <summary>
/// A generic function executed periodically.
/// </summary>
public class Interval
{
    private static readonly Cache _cache = new();

    /// <summary>
    /// Invoke an <see cref="Action"/> periodically.
    /// </summary>
    /// <param name="name">A name for the interval.</param>
    /// <param name="function">The <see cref="Action"/> to invoke.</param>
    /// <param name="interval">The rate at which to invoke the <see cref="Action"/>.</param>
    public static void Schedule(
        string name,
        Action function,
        Time interval)
    {
        var now = Time.Now;

        if (!_cache.TryGet<double>(Constants.Intervals, name, out var time) || (now - time) >= interval)
        {
            _cache.Set(Constants.Intervals, name, now.Milliseconds, Time.FromDays(1));

            function.Invoke();
        }
    }

    /// <summary>
    /// Invoke an <see cref="Action"/> in the background periodically.
    /// </summary>
    /// <param name="name">A name for the interval.</param>
    /// <param name="function">The <see cref="Action"/> to invoke.</param>
    /// <param name="interval">The rate at which to invoke the <see cref="Action"/>.</param>
    public static void ScheduleAsync(
        string name,
        Action function,
        Time interval)
    {
        Schedule(name, () => Task.Run(function), interval);       
    }
}