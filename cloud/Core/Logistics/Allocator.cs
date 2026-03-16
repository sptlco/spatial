// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Hosting;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Spatial.Logistics.Helpers;
using Spatial.Persistence;
using System.Numerics;

namespace Spatial.Logistics;

/// <summary>
/// An Ethereum stable coin allocator.
/// </summary>
public class Allocator : BackgroundService
{
    private const string USDC = "0xA0b86991c6218b36c1d19D4a2e9Eb0cE3606eB48";
    private const string WETH = "0xC02aaA39b223FE8D0A0e5C4F27eAD9083C756Cc2";

    private readonly Ethereum _ethereum;
    private readonly Cache _cache;
    private readonly AllocatorConfiguration _config;

    private decimal _costBasis;

    private DateTime _bought
    {
        get => _cache.TryGet<DateTime>("allocator", "bought", out var value) ? value : DateTime.MinValue;
        set => _cache.Set("allocator", "bought", value);
    }

    private DateTime _sold
    {
        get => _cache.TryGet<DateTime>("allocator", "sold", out var value) ? value : DateTime.MinValue;
        set => _cache.Set("allocator", "sold", value);
    }

    private DateTime _tookProfit
    {
        get => _cache.TryGet<DateTime>("allocator", "took-profit", out var value) ? value : DateTime.MinValue;
        set => _cache.Set("allocator", "took-profit", value);
    }

    /// <summary>
    /// Create a new <see cref="Allocator"/>.
    /// </summary>
    public Allocator()
    {
        _ethereum = Ethereum.CreateClient();
        _cache = new Cache();
        _config = Configuration.Current.Ethereum.Allocator;
    }

    /// <summary>
    /// Execute the service.
    /// </summary>
    /// <param name="token">The service's <see cref="CancellationToken"/>.</param>
    protected override async Task ExecuteAsync(CancellationToken token)
    {
        _costBasis = await ComputeCostBasisAsync(_tookProfit);

        while (!token.IsCancellationRequested)
        {
            var next = DateTime.UtcNow.AddMinutes(1);

            try
            {
                var price = await _ethereum.GetPriceAsync(Constants.Contracts.CHAINLINK_ETH_USD, 8);
                var ethereum = Web3.Convert.FromWei(await _ethereum.GetBalanceAsync());
                var dollars = (decimal) (await _ethereum.GetERC20DetailsAsync(USDC))[USDC].Balance / (decimal) Math.Pow(10, 6);
                var value = (Ethereum: ethereum * price, Total: (ethereum * price) + dollars);

                if (value.Total > 0)
                {
                    var weight = value.Ethereum / value.Total;
                    var upper = _config.Exposure + _config.Bandwidth;
                    var lower = _config.Exposure - _config.Bandwidth;

                    if (_config.Profit.Gain > 0 && _costBasis > 0)
                    {
                        var gain = (price - _costBasis) / _costBasis;

                        if (gain >= _config.Profit.Gain && value.Ethereum > _config.Minimum && DateTime.UtcNow - _sold > _config.Cooldown)
                        {
                            INFO("Taking profit at {Gain:P2} gain. Cost basis: {CostBasis}, Current price: {Price}.", gain, _costBasis, price);

                            var reserve = await EstimateGasReserveAsync(value.Ethereum, price);
                            var amount = value.Ethereum - (reserve * price);

                            await SellAsync(amount, price);

                            _sold = DateTime.UtcNow;
                            _tookProfit = DateTime.UtcNow;
                            _costBasis = 0;
                        }
                    }
                    else if (weight > upper)
                    {
                        var excess = value.Ethereum - (value.Total * _config.Exposure);

                        if (excess > _config.Minimum && DateTime.UtcNow - _sold > _config.Cooldown)
                        {
                            await SellAsync(excess, price);

                            _sold = DateTime.UtcNow;
                        }
                    }
                    else if (weight < lower)
                    {
                        var deficit = (value.Total * _config.Exposure) - value.Ethereum;

                        if (deficit > _config.Minimum && DateTime.UtcNow - _bought > _config.Cooldown && DateTime.UtcNow - _tookProfit > _config.Profit.Cooldown)
                        {
                            await BuyAsync(deficit, price);

                            _bought = DateTime.UtcNow;
                        }
                    }
                }

                await Metric.WriteOneAsync("ethereum", new {
                    Balance = ethereum,
                    Price = price
                });
            }
            catch (RpcClientUnknownException)
            {
                WARN("Failed to analyze Ethereum due to a transient error.");
            }
            catch (Exception e)
            {
                ERROR(e, "Failed to analyze Ethereum due to an unexpected error.");
            }
            finally
            {
                await Task.Delay(TimeSpan.FromMilliseconds(Math.Max((next - DateTime.UtcNow).TotalMilliseconds, 10)), token);
            }
        }
    }

    private async Task BuyAsync(decimal amount, decimal price)
    {
        var amountIn = (BigInteger)(amount * (decimal)Math.Pow(10, 6));

        if (amountIn <= 0)
        {
            WARN("Insufficient USDC for Ethereum purchase.");
            return;
        }

        string[] path = [USDC, WETH];

        try
        {
            await _ethereum.ApproveAsync(USDC, Constants.Contracts.UniswapV2Router02, amountIn);

            var amountsOut = await Uniswap.GetAmountsOutAsync(amountIn, path);
            var slippage = (BigInteger)((1.0M - _config.Tolerance) * 10_000.0M);
            var amountOutMin = amountsOut.Last() * slippage / 10_000;
            var deadline = (uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + _config.Deadline * 60;
            var timestamp = Time.Now;

            var receipt = await Uniswap.SwapExactTokensForETHAsync(
                amountIn,
                amountOutMin,
                path,
                _ethereum.Account.Address,
                deadline);

            await Metric.WriteOneAsync(
                name: "transaction",
                value: new {
                    Duration = (decimal)(Time.Now - timestamp),
                    Volume = amount,
                    Price = price,
                    Gas = Web3.Convert.FromWei(receipt.GasUsed.Value * receipt.EffectiveGasPrice.Value) * price
                },
                metadata: new {
                    Hash = receipt.TransactionHash,
                    Direction = "BUY",
                    Pair = "USDC/ETH"
                });

            _costBasis = await ComputeCostBasisAsync(_tookProfit);

            INFO("Purchased {Volume} Ethereum at {Price}: {Transaction}", amount, price, receipt.TransactionHash);
        }
        catch (Exception e)
        {
            ERROR(e, "Failed to purchase Ethereum due to an unexpected error.");
        }
    }

    private async Task SellAsync(decimal amount, decimal price)
    {
        var amountIn = Web3.Convert.ToWei(amount / price);

        if (amountIn <= 0)
        {
            WARN("Insufficient Ethereum for sale.");
            return;
        }

        string[] path = [WETH, USDC];

        try
        {
            var amountsOut = await Uniswap.GetAmountsOutAsync(amountIn, path);
            var slippage = (BigInteger)((1.0M - _config.Tolerance) * 10_000.0M);
            var amountOutMin = amountsOut.Last() * slippage / 10_000;
            var deadline = (uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + _config.Deadline * 60;
            var timestamp = Time.Now;

            var receipt = await Uniswap.SwapExactETHForTokensAsync(
                amountIn,
                amountOutMin,
                path,
                _ethereum.Account.Address,
                deadline);

            await Metric.WriteOneAsync(
                name: "transaction",
                value: new {
                    Duration = (decimal)(Time.Now - timestamp),
                    Volume = -amount,
                    Price = price,
                    Gas = Web3.Convert.FromWei(receipt.GasUsed.Value * receipt.EffectiveGasPrice.Value) * price
                },
                metadata: new {
                    Hash = receipt.TransactionHash,
                    Direction = "SELL",
                    Pair = "ETH/USDC"
                });

            INFO("Sold {Volume} Ethereum at {Price}: {Transaction}", amount, price, receipt.TransactionHash);
        }
        catch (Exception e)
        {
            ERROR(e, "Failed to sell Ethereum due to an unexpected error.");
        }
    }

    private static async Task<decimal> ComputeCostBasisAsync(DateTime since)
    {
        var transactions = await Metric.ReadAsync("transaction", from: since, resolution: "1m");
        var buys = transactions.Where(t => t.Value.TryGetValue("Volume", out var value) && value > 0).ToList();

        if (buys.Count == 0)
        {
            return 0;
        }

        var dollars = buys.Sum(t => t.Value["Volume"]);
        var ethereum = buys.Sum(t => t.Value["Volume"] / t.Value["Price"]);

        return ethereum > 0 ? dollars / ethereum : 0;
    }

    private async Task<decimal> EstimateGasReserveAsync(decimal amount, decimal price)
    {
        var amountIn = Web3.Convert.ToWei(amount / price);
        
        string[] path = [WETH, USDC];

        var amountsOut = await Uniswap.GetAmountsOutAsync(amountIn, path);
        var slippage = (BigInteger)((1.0M - _config.Tolerance) * 10_000.0M);
        var amountOutMin = amountsOut.Last() * slippage / 10_000;
        var deadline = (uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + _config.Deadline * 60;

        var units = await Uniswap.EstimateSwapExactETHForTokensGasAsync(
            amountIn,
            amountOutMin,
            path,
            _ethereum.Account.Address,
            deadline);

        var gasPrice = await _ethereum.Web3.Eth.GasPrice.SendRequestAsync();
        var gasCost = Web3.Convert.FromWei(units * gasPrice.Value);

        return gasCost * _config.Profit.Buffer;
    }
}

/// <summary>
/// Configurable options for the <see cref="Allocator"/>.
/// </summary>
public class AllocatorConfiguration
{
    /// <summary>
    /// Whether or not the <see cref="Allocator"/> is enabled.
    /// </summary>
    public bool Enabled { get; set; } = false;

    /// <summary>
    /// The percentage of dollars allocated to Ethereum.
    /// </summary>
    public decimal Exposure { get; set; }

    /// <summary>
    /// The allowed deviation from the target weight.
    /// </summary>
    public decimal Bandwidth { get; set; }

    /// <summary>
    /// The minimum trade size in dollars.
    /// </summary>
    public decimal Minimum { get; set; }

    /// <summary>
    /// A trade deadline in minutes.
    /// </summary>
    public uint Deadline { get; set; }

    /// <summary>
    /// The amount of slippage to tolerate.
    /// </summary>
    public decimal Tolerance { get; set; }

    /// <summary>
    /// The allocator's cooldown per order direction.
    /// </summary>
    public TimeSpan Cooldown { get; set; } = TimeSpan.FromMinutes(30);

    /// <summary>
    /// Configurable options for the allocator's take-profit mechanism.
    /// </summary>
    public ProfitConfiguration Profit { get; set; } = new ProfitConfiguration();

    /// <summary>
    /// Configurable options for the allocator's take-profit mechanism.
    /// </summary>
    public class ProfitConfiguration
    {
        /// <summary>
        /// The unrealized gain at which to exit the entire ETH position. 0 = disabled.
        /// </summary>
        public decimal Gain { get; set; } = 0;

        /// <summary>
        /// How long to stay out of ETH after taking profit before rebalancing back in.
        /// </summary>
        public TimeSpan Cooldown { get; set; } = TimeSpan.FromHours(24);

        /// <summary>
        /// A multiplier applied to the estimated gas cost to account for price fluctuations.
        /// </summary>
        public decimal Buffer { get; set; } = 1.2M;
    }
}