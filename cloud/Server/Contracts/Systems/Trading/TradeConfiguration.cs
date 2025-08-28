// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Contracts.Systems.Trading;

/// <summary>
/// Configurable options for automated trading.
/// </summary>
public class TradeConfiguration
{
    /// <summary>
    /// Whether or not automated trading is enabled.
    /// </summary>
    public bool Enabled { get; set; } = false;
}