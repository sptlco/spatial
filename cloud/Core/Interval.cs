// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Caching;
using Spatial.Extensions;

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
    /// <param name="function">The <see cref="Action"/> to invoke.</param>
    /// <param name="interval">The rate at which to invoke the <see cref="Action"/>.</param>
    public static void Schedule(Action function, Time interval)
    {
        var now = Time.Now;
        var key = function.GetOrCreateIdentifier();

        if (!_cache.TryGet<double>(Constants.Intervals, key, out var time) || (now - time) >= interval)
        {
            _cache.Set(Constants.Intervals, key, now.Milliseconds, Time.FromDays(1));

            function.Invoke();
        }
    }

    /// <summary>
    /// Invoke an <see cref="Action"/> in the background periodically.
    /// </summary>
    /// <param name="function">The <see cref="Action"/> to invoke.</param>
    /// <param name="interval">The rate at which to invoke the <see cref="Action"/>.</param>
    public static void ScheduleAsync(Action function, Time interval)
    {
        Schedule(() => Task.Run(function), interval);       
    }
}