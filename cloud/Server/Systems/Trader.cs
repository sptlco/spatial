// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Blockchain;
using Spatial.Contracts;
using Spatial.Simulation;

namespace Spatial.Systems;

/// <summary>
/// An automated trading bot on the Ethereum blockchain.
/// </summary>
[Dependency]
public class Trader : System
{
    private readonly CloudConfiguration _config;
    private readonly Ethereum _ethereum;

    /// <summary>
    /// Create a new <see cref="Trader"/>.
    /// </summary>
    /// <param name="config">Configurable options for the <see cref="Trader"/>.</param>
    public Trader(CloudConfiguration config)
    {
        _config = config;

        if (_config.Systems.Trader.Enabled)
        {
            _ethereum = Ethereum.GetOrCreateClient();
        }
    }

    /// <summary>
    /// Update the <see cref="Space"/>.
    /// </summary>
    /// <param name="space">The <see cref="Space"/> to update.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public override void Update(Space space, Time delta)
    {
        if (_config.Systems.Trader.Enabled)
        {
            Interval.Invoke(Trade, Time.FromMinutes(1));
        }
    }

    private void Trade()
    {
        // Since transactions and function calls may take some time, perform 
        // the trade operation asynchronously (so as to not block the application's core update loop).

        Task.Run(async () =>
        {
            var balance = await _ethereum.GetBalanceAsync();

            // If we have capital available, and enough to invest, make an investment by 
            // executing the following strategy:
            // ...

            if (balance >= _config.Systems.Trader.MinimumInvestment)
            {
                // ...
            }

            // At times, it may make sense to sell some tokens (swap them for ETH) so that 
            // the system can buy more.

            // ...
        });
    }
}