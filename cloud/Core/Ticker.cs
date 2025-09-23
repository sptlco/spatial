// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

/// <summary>
/// A temporal device that executes a function periodically.
/// </summary>
public class Ticker
{
    /// <summary>
    /// Run a <see cref="Ticker"/>.
    /// </summary>
    /// <param name="function">A function to perform each tick.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/>.</param>
    public static void Run(
        Action<Time> function,
        CancellationToken cancellationToken = default)
    {
        var t0 = Time.Now;

        while (!cancellationToken.IsCancellationRequested)
        {
            var time = Time.Now;
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
        var next = Time.Now;

        while (!cancellationToken.IsCancellationRequested)
        {
            var now = Time.Now;

            if (now >= next)
            {
                function(delta);
                next += delta;
            }

            Thread.Sleep((int) Math.Max(next - Time.Now, 1));
        }
    }
}