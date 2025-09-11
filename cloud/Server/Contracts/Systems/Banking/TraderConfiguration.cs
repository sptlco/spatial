// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Systems.Banking;

namespace Spatial.Cloud.Contracts.Systems.Banking;

/// <summary>
/// Configurable options for the <see cref="Trader"/>.
/// </summary>
internal class TraderConfiguration
{
    /// <summary>
    /// Whether or not the <see cref="Trader"/> is enabled.
    /// </summary>
    public bool Enabled { get; set; } = false;

    /// <summary>
    /// The interval at which the <see cref="Trader"/> trades.
    /// </summary>
    public TimeSpan Interval { get; set; } = TimeSpan.FromHours(1);

    /// <summary>
    /// The confidence required to executge a trade.
    /// </summary>
    public int ConfidenceThreshold { get; set; } = 60;

    /// <summary>
    /// The maximum number of trades the <see cref="Trader"/> will attempt to execute 
    /// per trading cycle.
    /// </summary>
    public int MaxTradesPerCycle { get; set; } = 2;

    /// <summary>
    /// A list of ERC20 tokens watched by the <see cref="Trader"/>.
    /// </summary>
    public Dictionary<string, string> Watchlist { get; set; } = [];
}