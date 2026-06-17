// Copyright © Spatial Corporation. All rights reserved.

using System.Diagnostics;

namespace Spatial;

/// <summary>
/// A static function profiler.
/// </summary>
public class Profiler
{
    /// <summary>
    /// Measure how long an <see cref="Action"/> takes.
    /// </summary>
    /// <param name="name">The name of the measurement.</param>
    /// <param name="action">An <see cref="Action"/>.</param>
    /// <returns>The number of milliseconds the <see cref="Action"/> took.</returns>
    public static (string Name, double Elapsed) Measure(string name, Action action)
    {
        var start = Stopwatch.GetTimestamp();

        action();

        return (name, (Stopwatch.GetTimestamp() - start) * 1000.0D / Stopwatch.Frequency);
    }
}