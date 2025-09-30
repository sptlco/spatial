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
    /// <param name="background">Whether or not to execute the function in the background.</param>
    public static void Schedule(Func<Task> function, Time interval, bool background = false)
    {
        var now = Time.Now;
        var key = function.GetOrCreateIdentifier();

        if (!_cache.TryGet<double>(Constants.Intervals, key, out var time) || (now - time) >= interval)
        {
            _cache.Set(Constants.Intervals, key, now.Milliseconds, Time.FromDays(1));

            if (background)
            {
                Task.Run(function);
                return;
            }

            function().GetAwaiter().GetResult();
        }
    }

    /// <summary>
    /// Invoke an <see cref="Action"/> in the background periodically.
    /// </summary>
    /// <param name="function">The <see cref="Action"/> to invoke.</param>
    /// <param name="interval">The rate at which to invoke the <see cref="Action"/>.</param>
    public static void ScheduleAsync(Func<Task> function, Time interval) => Schedule(function, interval, true);
}