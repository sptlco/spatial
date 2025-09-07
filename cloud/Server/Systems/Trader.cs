// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Simulation;

namespace Spatial.Cloud.Systems;

/// <summary>
/// An automated trading bot on the Ethereum blockchain.
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
        // the trade operation asynchronously (don't block the main thread).

        Task.Run(Strategize);
    }

    private async Task Strategize()
    {
        // ...
    }
}

/// <summary>
/// Configurable options for the <see cref="Trader"/>.
/// </summary>
internal class TraderConfiguration
{
    /// <summary>
    /// Whether or not the <see cref="Trader"/> is enabled.
    /// </summary>
    public bool Enabled { get; set; } = false;
}