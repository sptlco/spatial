// Copyright © Spatial Corporation. All rights reserved.

using System.Collections.Concurrent;

namespace Spatial;

/// <summary>
/// A generic function executed periodically.
/// </summary>
public class Interval
{
    private static readonly ConcurrentDictionary<string, Time> _intervals = [];

    /// <summary>
    /// Invoke an <see cref="Action"/> periodically.
    /// </summary>
    /// <param name="name">A name for the interval.</param>
    /// <param name="function">The <see cref="Action"/> to invoke.</param>
    /// <param name="interval">The rate at which to invoke the <see cref="Action"/>.</param>
    public static void Invoke(
        string name,
        Action function,
        Time interval)
    {
        var now = Time.Now;

        if (!_intervals.TryGetValue(name, out var time) || (now - time) >= interval)
        {
            _intervals[name] = now;

            function.Invoke();
        }
    }

    /// <summary>
    /// Invoke an <see cref="Action"/> in the background periodically.
    /// </summary>
    /// <param name="name">A name for the interval.</param>
    /// <param name="function">The <see cref="Action"/> to invoke.</param>
    /// <param name="interval">The rate at which to invoke the <see cref="Action"/>.</param>
    public static void FireAndForget(
        string name,
        Action function,
        Time interval)
    {
        Invoke(name, () => Task.Run(function), interval);       
    }
}