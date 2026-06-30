// Copyright © Spatial Corporation. All rights reserved.

using System.Collections.Concurrent;

namespace Spatial;

/// <summary>
/// Tests for a <see cref="Ticker"/>.
/// </summary>
public class TickerTests
{
    /// <summary>
    /// Run a <see cref="Ticker"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestRun()
    {
        var ticker = new Ticker();
        var token = new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token;
        var ticks = new ConcurrentBag<double>();

        ticker.Run(
            cancellationToken: token,
            delta: (1 / 60D) * 1000D,
            function: delta => {
                ticks.Add(Time.Now);
            });

        Assert.True(ticks.Count >= 160);
    }

    /// <summary>
    /// Run a <see cref="Ticker"/> at an uncapped rate.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestRunUncapped()
    {
        var ticker = new Ticker();
        var token = new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token;
        var ticks = new ConcurrentBag<double>();

        ticker.Run(
            cancellationToken: token,
            function: delta => {
                ticks.Add(Time.Now);
            });

        Assert.True(ticks.Count >= 180);
    }
}