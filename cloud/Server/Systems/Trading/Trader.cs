// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Blockchain;
using Spatial.Contracts;
using Spatial.Simulation;

namespace Spatial.Systems.Trading;

/// <summary>
/// Automated trading by way of token swaps.
/// </summary>
[Dependency]
public class Trader : System
{
    private readonly IOptionsMonitor<CloudConfiguration> _config;
    private readonly Ethereum _ethereum;

    /// <summary>
    /// Create a new <see cref="Trader"/>.
    /// </summary>
    /// <param name="config">Configurable options for the <see cref="Trader"/>.</param>
    public Trader(IOptionsMonitor<CloudConfiguration> config)
    {
        _config = config;

        if (_config.CurrentValue.Systems.Trading.Enabled)
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
        if (_config.CurrentValue.Systems.Trading.Enabled)
        {
            Interval.Invoke(Trade, Time.FromMinutes(1));
        }
    }

    private void Trade()
    {
        // ...
    }
}