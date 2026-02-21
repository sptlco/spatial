// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Hosting;
using Nethereum.Web3;
using Polly;
using Polly.Retry;
using Spatial.Blockchain.Helpers;
using Spatial.Extensions;

namespace Spatial.Blockchain;

/// <summary>
/// Monitors and manages the <see cref="Ethereum"/> account.
/// </summary>
public class Trader : BackgroundService
{
    private readonly Ethereum _ethereum;
    private readonly ResiliencePipeline _retry;
    private CoinGecko.Coin _coin;

    /// <summary>
    /// Create a new <see cref="Trader"/>.
    /// </summary>
    public Trader()
    {
        _ethereum = Ethereum.CreateClient();
        _retry = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions())
            .Build();
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
                _coin = (await CoinGecko.GetMarketsAsync(["ethereum"]))[0];

                // Take a snapshot of the current account balance.
                // Use Polly to ensure success.

                await _retry.Execute(Snapshot);
            }
            catch (Exception e)
            {
                WARN(e, "Trader failed to execute due to a transient error.");
            }
            finally
            {
                await Task.Delay(next - DateTime.UtcNow, token);
            }
        }
    }

    private async Task Snapshot()
    {
        var wei = await _ethereum.GetBalanceAsync();
        var eth = Web3.Convert.FromWei(wei);

        var metric = new Metric { 
            Metadata = new Dictionary<string, string> {
                { "name", "ethereum" },
            },
            Value = new Dictionary<string, decimal> {
                { "balance", eth },
                { "price", _coin.Price }
            }
        };

        metric.Store();
    }
}