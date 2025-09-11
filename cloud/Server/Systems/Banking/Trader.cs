// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Blockchain;
using Spatial.Blockchain.Helpers;
using Spatial.Cloud.Contracts;
using Spatial.Simulation;
using System.Numerics;
 
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
        var start = Time.Now;
        var next = Time.FromMilliseconds(start + _config.Systems.Banking.Trader.Interval.TotalMilliseconds).ToDateTime().ToLocalTime();

        INFO("Trade dependant on analysis.");

        try
        {
            var coins = await GetCoinsAsync();
            var ethereum = coins.First(c => c.Id == Constants.Ethereum);
            var recommendations = await Analyzer.AnalyzeAsync(coins);
            var threshold = _config.Systems.Banking.Trader.ConfidenceThreshold;
            var max = _config.Systems.Banking.Trader.MaxTradesPerCycle;

            INFO("Completed trade analysis with {Recommendations} recommendations.", recommendations.Count);

            recommendations = [.. recommendations
                .Where(rec => rec.Confidence >= threshold)
                .OrderByDescending(rec => rec.Confidence)
                .Take(max)];

            if (recommendations.Count <= 0)
            {
                WARN("Trading requires at most {Trades} recommendations with confidence over {Threshold}.", max, threshold);
            }
            else
            {
                INFO("Executing top {Trades} trade recommendations.", recommendations.Count);

                foreach (var trade in recommendations)
                {
                    var coin = coins.First(coin => coin.Id == trade.Coin);
                    var decimals = await Ethereum.GetOrCreateClient().GetDecimalsAsync(coin.Address);
                    var size = (BigInteger) ((double) (trade.Action == TradeAction.Buy ? ethereum.Balance : coin.Balance) * trade.Size);

                    var transaction = await (trade.Action switch {
                        TradeAction.Buy => Broker.BuyAsync(coin, size),
                        TradeAction.Sell => Broker.SellAsync(coin, size),
                    });

                    decimal GetReadableSize()
                    {
                        return trade.Action switch {
                            TradeAction.Sell => (decimal) size / (decimal) Math.Pow(10, decimals),
                            TradeAction.Buy => (decimal) size / (decimal) 1e18 * ethereum.Price / coin.Price
                        };
                    }
                    
                    INFO("{Action} {Size} {Symbol} at {Price} USD: {Transaction}", trade.Action, GetReadableSize(), coin.Symbol.ToUpper(), coin.Price, transaction);
                }
            }

            INFO("Trade complete, next at {Time}.", next);
        }
        catch (Exception exception)
        {
            ERROR(exception, "Trade failed, next at {Time}.", next);
        }
    }

    private async Task<List<CoinGecko.Coin>> GetCoinsAsync()
    {
        var ethereum = Ethereum.GetOrCreateClient();
        var watchlist = _config.Systems.Banking.Trader.Watchlist;
        var coins = await CoinGecko.GetMarketsAsync([Constants.Ethereum, .. watchlist.Keys]);
        var funds = await ethereum.GetBalanceAsync();
        var balances = await ethereum.GetERC20BalancesAsync([.. watchlist.Values]);

        foreach (var coin in coins)
        {
            if (watchlist.TryGetValue(coin.Id, out var address))
            {
                coin.Address = address;
            }

            coin.Balance = !string.IsNullOrEmpty(coin.Address) ? balances[coin.Address] : funds;
        }

        return coins;
    }
}