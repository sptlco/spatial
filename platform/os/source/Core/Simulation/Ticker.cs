// Copyright © Spatial. All rights reserved.

namespace Spatial.Simulation;

/// <summary>
/// A <see cref="Time"/> device that performs routine functions 
/// at a fixed interval.
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
        }
    }
    
    /// <summary>
    /// Run a <see cref="Ticker"/>.
    /// </summary>
    /// <param name="function">A function to perform each tick.</param>
    /// <param name="rate">The rate at which to perform <paramref name="function"/>.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/>.</param>
    public static void Run(
        Action<Time> function, 
        Time rate, 
        CancellationToken cancellationToken = default)
    {
        var t0 = Time.Now;
        var accumulated = Time.Zero;

        while (!cancellationToken.IsCancellationRequested)
        {
            var t = Time.Now;
            var elapsed = t - t0;

            accumulated += elapsed;
            t0 = t;

            while (!cancellationToken.IsCancellationRequested && accumulated >= rate)
            {
                accumulated -= rate;
                function(rate);
            }
        }
    }
}