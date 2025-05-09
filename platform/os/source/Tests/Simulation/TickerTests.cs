// Copyright © Spatial. All rights reserved.

namespace Spatial.Simulation;

/// <summary>
/// Unit tests for <see cref="Ticker"/>.
/// </summary>
public class TickerTests
{
    /// <summary>
    /// Test <see cref="Ticker.Run(Action{Time}, Time, CancellationToken)"/>.
    /// </summary>
    [Fact]
    public void TestRun()
    {
        var ticks = 0;

        Ticker.Run(
            rate: (long) (1 / 30D * 1000),
            cancellationToken: new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token,
            function: (delta) => {
                ticks += 1;
            }
        );

        Assert.True(ticks > 0);
    }
}
