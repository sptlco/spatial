// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Systems;

namespace Spatial.Contracts.Systems;

/// <summary>
/// Configurable options for automated trading.
/// </summary>
public class TraderConfiguration
{
    /// <summary>
    /// Whether or not the <see cref="Trader"/> is enabled.
    /// </summary>
    public bool Enabled { get; set; } = false;

    /// <summary>
    /// The minimum amount of Ethereum to invest.
    /// </summary>
    public uint MinimumInvestment { get; set; } = 1;
}