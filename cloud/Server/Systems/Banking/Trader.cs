// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts;
using Spatial.Simulation;
 
namespace Spatial.Cloud.Systems.Banking;

/// <summary>
/// Automated trading on the Ethereum network.
/// </summary>
[Dependency]
internal class Trader : System
{
    private readonly ServerConfiguration _config;

    /// <summary>
    /// Create a new <see cref="Trader"/>.
    /// </summary>
    /// <param name="config">Configurable options for the <see cref="Trader"/>.</param>
    public Trader(ServerConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Update the <see cref="Trader"/>.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public override void Update(Space space, Time delta)
    {
        if (_config.Systems.Banking.Trader.Enabled)
        {
            // Since transactions and function calls may take some time, perform 
            // the trade operation asynchronously (don't block the main thread).

            Interval.FireAndForget(Constants.Intervals.Trade, Trade, Time.FromMilliseconds(_config.Systems.Banking.Trader.Interval.TotalMilliseconds));
        }
    }

    private async void Trade()
    {
        INFO("Trade dependant on analysis.");

        try
        {
            var data = await Analyzer.AnalyzeAsync(_config.Systems.Banking.Trader.Watchlist);

            foreach (var analysis in data)
            {
                if ((analysis.Score is > -1.0D and < 1.0D) || analysis.Size <= 0.0M)
                {
                    continue;
                }

                if (analysis.Score <= -1.0D)
                {
                    INFO("Swap {Size} {Coin} for ETH at {Price} USD.", analysis.Size, analysis.Coin.Symbol.ToUpper(), analysis.Coin.Price);

                    // ...
                }
                else if (analysis.Score >= 1.0D)
                {
                    INFO("Swap {Size} ETH for {Coin} at {Price} USD.", analysis.Size, analysis.Coin.Symbol.ToUpper(), analysis.Coin.Price);

                    // ...
                }
            }
        }
        catch (Exception exception)
        {
            ERROR(exception, "Failed to trade due to an unhandled exception.");
        }
    }
}