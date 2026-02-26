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
    private readonly AllocatorConfiguration _config;

    /// <summary>
    /// Create a new <see cref="Allocator"/>.
    /// </summary>
    public Allocator()
    {
        _ethereum = Ethereum.CreateClient();
        _config = Configuration.Current.Ethereum.Allocator;
    }

    /// <summary>
    /// Execute the service.
    /// </summary>
    /// <param name="token">The service's <see cref="CancellationToken"/>.</param>
    protected override async Task ExecuteAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var next = DateTime.UtcNow.AddMinutes(1);

            try
            {
                var price = await _ethereum.GetPriceAsync(Constants.Contracts.CHAINLINK_ETH_USD, 8);
                var details = (await _ethereum.GetERC20DetailsAsync(USDC))[USDC];
                var ethereum = Web3.Convert.FromWei(await _ethereum.GetBalanceAsync());
                var dollars = (decimal) details.Balance / (decimal) Math.Pow(10, 6);
                var value = (Ethereum: ethereum * price, Total: (ethereum * price) + dollars);

                if (value.Total > 0)
                {
                    var weight = value.Ethereum / value.Total;
                    var upper = _config.Exposure + _config.Bandwidth;
                    var lower = _config.Exposure - _config.Bandwidth;

                    if (weight > upper)
                    {
                        var target = value.Total * _config.Exposure;
                        var excess = value.Ethereum - target;

                        if (excess > _config.Minimum)
                        {
                            await SellAsync(excess, price);
                        }
                    }
                    else if (weight < lower)
                    {
                        var target = value.Total * _config.Exposure;
                        var deficit = target - value.Ethereum;

                        if (deficit > _config.Minimum)
                        {
                            await BuyAsync(deficit, price);
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
        var amountIn = (BigInteger) (amount * (decimal) Math.Pow(10, 6));

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
            var deadline = (uint) DateTimeOffset.UtcNow.ToUnixTimeSeconds() + _config.Deadline * 60;
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
                    Duration = (decimal) (Time.Now - timestamp),
                    Volume = amount,
                    Price = price,
                    Gas = Web3.Convert.FromWei(0) * price
                },
                metadata: new {
                    Hash = "0x012345566611",
                    Direction = "BUY",
                    Pair = "USDC/ETH"
                });
            
            INFO("Purchased Ethereum: {Transaction}", "0x012345566611");
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
            var slippage = (BigInteger) ((1.0M - _config.Tolerance) * 10_000.0M);
            var amountOutMin = amountsOut.Last() * slippage / 10_000;
            var deadline = (uint) DateTimeOffset.UtcNow.ToUnixTimeSeconds() + _config.Deadline * 60;
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
                    Duration = (decimal) (Time.Now - timestamp),
                    Volume = -amount,
                    Price = price,
                    Gas = Web3.Convert.FromWei(0) * price
                },
                metadata: new {
                    Hash = "0x012345566611",
                    Direction = "SELL",
                    Pair = "ETH/USDC"
                });

            INFO("Sold Ethereum for USDC: {Transaction}", "0x012345566611");
        }
        catch (Exception e)
        {
            ERROR(e, "Failed to sell Ethereum due to an unexpected error.");
        }
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
}