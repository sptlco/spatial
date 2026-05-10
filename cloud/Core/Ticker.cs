// Copyright © Spatial Corporation. All rights reserved.

using System.Diagnostics;

namespace Spatial;

/// <summary>
/// A temporal device that executes a function periodically.
/// </summary>
public class Ticker
{
    private static readonly double _factor = 1000.0 / Stopwatch.Frequency;
    private static double Now() => Stopwatch.GetTimestamp() * _factor;

    /// <summary>
    /// Run a <see cref="Ticker"/>.
    /// </summary>
    /// <param name="function">A function to perform each tick.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/>.</param>
    public static void Run(
        Action<Time> function,
        CancellationToken cancellationToken = default)
    {
        var t0 = Now();

        while (!cancellationToken.IsCancellationRequested)
        {
            var time = Now();
            var delta = time - t0;

            t0 = time;

            function(delta);

            Thread.Sleep(1);
        }
    }
    
    /// <summary>
    /// Run a <see cref="Ticker"/>.
    /// </summary>
    /// <param name="function">A function to perform each tick.</param>
    /// <param name="delta">The rate at which to perform <paramref name="function"/>.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/>.</param>
    public static void Run(
        Action<Time> function, 
        Time delta, 
        CancellationToken cancellationToken = default)
    {
        var next = Now();

        while (!cancellationToken.IsCancellationRequested)
        {
            var now = Now();

            // If we've fallen more than 3 ticks behind, accept the desync.
            // Rather than bursting to catch up.

            if (now - next > delta * 3)
            {
                next = now;
            }

            if (now >= next)
            {
                function(delta);
                next += delta;
            }

            Thread.Sleep((int) Math.Max(next - Now(), 1));
        }
    }
}