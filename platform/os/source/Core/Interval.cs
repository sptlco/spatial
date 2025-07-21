// Copyright © Spatial Corporation. All rights reserved.

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Spatial;

/// <summary>
/// A generic function executed periodically.
/// </summary>
public class Interval
{
    private static readonly ConcurrentDictionary<long, Time> _intervals = [];

    /// <summary>
    /// Invoke an <see cref="Action"/> periodically.
    /// </summary>
    /// <param name="function">The <see cref="Action"/> to invoke.</param>
    /// <param name="interval">The rate at which to invoke the <see cref="Action"/>.</param>
    /// <param name="file">The path to the file that declared the <see cref="Action"/>.</param>
    /// <param name="line">The line containing the <see cref="Action"/>.</param>
    public static void Invoke(
        Action function,
        Time interval,
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0)
    {
        var now = Time.Now;
        var key = GenerateKey(function, file, line);

        if (!_intervals.TryGetValue(key, out var time) || (now - time) >= interval)
        {
            _intervals[key] = now;

            function.Invoke();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static long GenerateKey(Action function, string file, int line)
    {
        if (function.Target == null)
        {
            return ((long) function.Method.MetadataToken << 32) | (uint) file.GetHashCode();
        }
        
        return ((long) RuntimeHelpers.GetHashCode(function.Target) << 32) | (uint) (line ^ file.GetHashCode());
    }
}