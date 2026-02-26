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

    private DateTime? _bought;
    private DateTime? _sold;

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
                // Start by fetching market data from CoinGecko.
                // Query all the supported coins with price, market cap, volume and market related data.

                var coins = await CoinGecko.GetMarketsAsync();
                var details = (await _ethereum.GetERC20DetailsAsync(USDC))[USDC];

                var ethereum = new {
                    Market = coins.First(coin => coin.Id.Equals("ethereum")),
                    Balance = await _ethereum.GetBalanceAsync()
                };

                var dollar = new {
                    Market = coins.First(coin => coin.Id == "usd-coin"),
                    details.Balance
                };

                // Stablecoin mean reversion.
                // Detect USDC price deviation from peg to 1 USD.

                var deviation = (dollar.Market.Price - 1.0M) / 1.0M;

                if (deviation < -_config.DeviationThreshold)
                {
                    // The dollar is cheap.
                    // Buy with Ethereum.

                    if (_bought is null || DateTime.UtcNow - _bought > _config.Cooldown)
                    {
                        await BuyAsync(ethereum.Balance, ethereum.Market.Price, deviation);

                        _bought = DateTime.UtcNow;
                    }
                    else
                    {
                        INFO("Ignoring BUY signal due to active cooldown.");
                    }
                }
                else if (deviation > _config.DeviationThreshold && dollar.Balance > 0)
                {
                    // The dollar is expensive.
                    // Sell for Ethereum.

                    if (_sold is null || DateTime.UtcNow - _sold > _config.Cooldown)
                    {
                        await SellAsync(dollar.Balance, ethereum.Market.Price, deviation);
                        
                        _sold = DateTime.UtcNow;
                    }
                    else
                    {
                        INFO("Ignoring SELL signal due to active cooldown.");
                    }
                }

                // Take a snapshot of the current state.
                // Use the new Metric API.

                await Metric.WriteManyAsync(new Dictionary<string, (object Value, object? Metadata)> {
                    ["ethereum"] = (
                        Value: new {
                            Balance = Web3.Convert.FromWei(ethereum.Balance),
                            ethereum.Market.Price
                        }, null),
                    ["dollar"] = (
                        Value: new {
                            Balance = (decimal) dollar.Balance / 1_000_000.0M,
                            dollar.Market.Price,
                            Deviation = deviation
                        }, null)
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

    private async Task BuyAsync(BigInteger capital, decimal price, decimal deviation)
    {
        var amountIn = Web3.Convert.ToWei(Web3.Convert.FromWei(capital) * _config.Capital);

        if (amountIn <= 0)
        {
            WARN("Insufficient Ethereum for USDC purchase.");
            return;
        }

        string[] path = [WETH, USDC];

        try
        {
            var amountsOut = await Uniswap.GetAmountsOutAsync(amountIn, path);
            var slippage = (BigInteger)((1.0M - _config.Tolerance) * 10_000.0M);
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
                    Price = price,
                    Deviation = deviation,
                    Volume = (decimal) amountsOut.Last() / 1_000_000.0M,
                    Slippage = _config.Tolerance,
                    Gas = Web3.Convert.FromWei(receipt.GasUsed.Value * receipt.EffectiveGasPrice.Value) * price
                },
                metadata: new {
                    Hash = receipt.TransactionHash,
                    Direction = "BUY",
                    Pair = "ETH/USDC"
                });

            INFO("Purchased USDC with Ethereum: {Transaction}", receipt.TransactionHash);
        }
        catch (Exception e)
        {
            ERROR(e, "Failed to purchase USDC due to an unexpected error.");
        }
    }

    private async Task SellAsync(BigInteger capital, decimal price, decimal deviation)
    {
        var amountIn = capital * (BigInteger) (_config.Capital * 1_000_000.0M) / 1_000_000;

        if (amountIn <= 0)
        {
            WARN("Insufficient USDC for sale.");
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
                    Price = price,
                    Deviation = deviation,
                    Volume = (decimal) amountIn / 1_000_000.0M,
                    Slippage = _config.Tolerance,
                    Gas = Web3.Convert.FromWei(receipt.GasUsed.Value * receipt.EffectiveGasPrice.Value) * price
                },
                metadata: new {
                    Hash = receipt.TransactionHash,
                    Direction = "SELL",
                    Pair = "USDC/ETH"
                });
            
            INFO("Sold USDC for Ethereum: {Transaction}", receipt);
        }
        catch (Exception e)
        {
            ERROR(e, "Failed to sell USDC due to an unexpected error.");
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
    /// The percentage of Ethereum available to the <see cref="Allocator"/> as capital.
    /// </summary>
    public decimal Capital { get; set; }

    /// <summary>
    /// Determines the stablecoin price deviation that signals an order.
    /// </summary>
    public decimal DeviationThreshold { get; set; }

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