// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Blockchain;
using Spatial.Blockchain.Helpers;
using Spatial.Cloud.Contracts;
using Spatial.Cloud.Contracts.Systems.Trading;
using Spatial.Cloud.Systems.Trading.Recommendations;
using Spatial.Simulation;
using System.Numerics;

namespace Spatial.Cloud.Systems.Trading;

/// <summary>
/// Automated trading on the Ethereum network.
/// </summary>
[Dependency]
internal class Trader : System
{
    private readonly ServerConfiguration _config;

    private long _interval;
    private byte _trading;

    /// <summary>
    /// Create a new <see cref="Trader"/>.
    /// </summary>
    /// <param name="config">Configurable options for the <see cref="Trader"/>.</param>
    public Trader(ServerConfiguration config)
    {
        _config = config;
        _interval = (long) _config.Systems.Trading.Trader.Interval.Period.TotalMilliseconds;
    }

    /// <summary>
    /// Update the <see cref="Trader"/>.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public override void Update(Space space, Time delta)
    {
        if (_config.Systems.Trading.Trader.Enabled)
        {
            // Since transactions and function calls may take some time, perform 
            // the trade operation asynchronously (don't block the main thread).

            Interval.ScheduleAsync(Trade, Interlocked.Read(ref _interval));
        }
    }

    private async Task Trade()
    {
        if (Interlocked.Exchange(ref _trading, 1) == 1)
        {
            return;
        }

        var start = Time.Now;

        try
        {
            var coins = await GetCoinsAsync();
            var interval = GetInterval(coins);
            var next = (Time) (start + interval);

            Interlocked.Exchange(ref _interval, interval);

            INFO("Trade dependant on analysis, instance: {This}.", this.GetHashCode());

            var portfolio = coins.Sum(coin => coin.Id == Constants.Ethereum ? 0 : coin.Value);
            var ethereum = coins.First(coin => coin.Id == Constants.Ethereum);

            if ((decimal) ethereum.Balance < _config.Systems.Trading.Trader.Reserve * 1e18M)
            {
                INFO("Insufficient funds: {Balance}/{Reserves} ETH.", (decimal) ethereum.Balance / 1e18M, _config.Systems.Trading.Trader.Reserve);
            }
            else
            {
                var funds = ethereum.Balance - new BigInteger(_config.Systems.Trading.Trader.Reserve * 1e18M);
                var recommendations = await Analyzer.AnalyzeAsync(coins);

                INFO("Completed trade analysis with {Recommendations} recommendations.", recommendations.Count);
                INFO("Executing {Trades} trade recommendations.", recommendations.Count);

                foreach (var trade in recommendations)
                {
                    var coin = coins.First(coin => coin.Id == trade.Coin);
                    var size = trade.Action switch {
                        TradeAction.Buy => (BigInteger) ((decimal) trade.Size * (decimal) funds),
                        TradeAction.Sell => (BigInteger) ((decimal) trade.Size * (decimal) coin.Balance)
                    };

                    decimal GetReadableSize()
                    {
                        return trade.Action switch {
                            TradeAction.Buy => (decimal) size / (decimal) 1e18 * ethereum.Price / coin.Price,
                            TradeAction.Sell => (decimal) size / (decimal) Math.Pow(10, coin.Decimals)
                        };
                    }

                    if (GetReadableSize() <= _config.Systems.Trading.Trader.MinimumTrade)
                    {
                        INFO("Insignificant trade: {Size} {Symbol}.", GetReadableSize(), trade.Action == TradeAction.Buy ? "ETH" : coin.Symbol.ToUpper());
                        continue;
                    }

                    try
                    {
                        var transaction = await (trade.Action switch {
                            TradeAction.Buy => Broker.BuyAsync(coin, size),
                            TradeAction.Sell => Broker.SellAsync(coin, size)
                        });

                        INFO("{Action} {Size} {Symbol} at {Price} USD: {Transaction}", trade.Action, GetReadableSize(), coin.Symbol.ToUpper(), coin.Price, transaction);
                    }
                    catch (Exception exception)
                    {
                        WARN(exception, "Transaction failed: {Action} {Size} {Symbol} at {Price} USD.", trade.Action, GetReadableSize(), coin.Symbol.ToUpper(), coin.Price);
                    }
                }

                INFO("Trade complete, next at {Time} ms.", next.Milliseconds);
            }
        }
        catch (Exception exception)
        {
            ERROR(exception, "Trade failed, next at {Time} ms.", ((Time) (start + _interval)).Milliseconds);
        }
        finally
        {
            Interlocked.Exchange(ref _trading, 0);
        }
    }

    private async Task<List<CoinGecko.Coin>> GetCoinsAsync()
    {
        var ethereum = Ethereum.CreateClient();
        var watchlist = _config.Systems.Trading.Trader.Watchlist;
        var coins = await CoinGecko.GetMarketsAsync([Constants.Ethereum, .. watchlist.Keys]);
        var funds = await ethereum.GetBalanceAsync();
        var details = await ethereum.GetERC20DetailsAsync([.. watchlist.Values]);

        foreach (var coin in coins)
        {
            if (watchlist.TryGetValue(coin.Id, out var address))
            {
                coin.Address = address;
            }

            if (!string.IsNullOrEmpty(coin.Address) && details.TryGetValue(coin.Address, out var deets))
            {
                coin.Balance = deets.Balance;
                coin.Decimals = deets.Decimals;
            }
            else
            {
                coin.Balance = funds;
                coin.Decimals = 18;
            }
        }

        return coins;
    }

    private long GetInterval(List<CoinGecko.Coin> coins)
    {
        return _config.Systems.Trading.Trader.Interval.Mode switch {
            TraderConfiguration.IntervalMode.Fixed => (long) _config.Systems.Trading.Trader.Interval.Period.TotalMilliseconds,
            TraderConfiguration.IntervalMode.Adaptive => ComputeAdaptiveInterval(coins)
        };
    }

    private long ComputeAdaptiveInterval(List<CoinGecko.Coin> coins)
    {
        var volatility = coins.Max(coin => (coin.High24H - coin.Low24H) / coin.Price);
        var period = _config.Systems.Trading.Trader.Interval.Period.TotalHours;
        var sensitivity = _config.Systems.Trading.Trader.Interval.Sensitivity;

        return (long) TimeSpan.FromHours(Math.Max(Constants.MinTradeIntervalHours, Math.Min(period, period / (1 + sensitivity * (double) volatility)))).TotalMilliseconds;
    }
}