// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Hosting;
using Nethereum.Web3;
using Spatial.Blockchain.Helpers;
using Spatial.Persistence;

namespace Spatial.Blockchain;

/// <summary>
/// Monitors and manages the <see cref="Ethereum"/> account.
/// </summary>
public class Trader : BackgroundService
{
    private readonly Ethereum _ethereum;

    /// <summary>
    /// Create a new <see cref="Trader"/>.
    /// </summary>
    public Trader()
    {
        _ethereum = Ethereum.CreateClient();
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
                var balance = await _ethereum.GetBalanceAsync();

                INFO("{@Coins}", coins);

                // ...

                // Take a snapshot of the current state.
                // Use Polly to ensure success.

                Resource<Metric>.Store(new Metric {
                    Metadata = new Dictionary<string, string> { ["name"] = "ethereum" },
                    Value = new Dictionary<string, decimal> {
                        ["balance"] = Web3.Convert.FromWei(balance),
                        ["price"] = 0
                    }
                });
            }
            catch (Exception e)
            {
                WARN(e, "Failed to execute trade due to a transient error.");
            }
            finally
            {
                await Task.Delay(TimeSpan.FromMilliseconds(Math.Max((next - DateTime.UtcNow).TotalMilliseconds, 10)), token);
            }
        }
    }
}