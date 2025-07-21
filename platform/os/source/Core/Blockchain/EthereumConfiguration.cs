// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Blockchain;

/// <summary>
/// Configurable options for the system's <see cref="Ethereum"/> provider.
/// </summary>
public class EthereumConfiguration
{
    /// <summary>
    /// Configurable options for automated trading.
    /// </summary>
    public TradeConfiguration Trades { get; set; } = new TradeConfiguration();

    /// <summary>
    /// Configurable options for automated trading.
    /// </summary>
    public class TradeConfiguration
    {
        /// <summary>
        /// Whether or not automated trading is enabled.
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
}