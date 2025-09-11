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
    /// A list of ERC20 tokens watched by the <see cref="Trader"/>.
    /// </summary>
    public Dictionary<string, string> Watchlist { get; set; } = [];
}