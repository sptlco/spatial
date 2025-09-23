// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Cloud.Systems.Trading;

namespace Spatial.Cloud.Contracts.Systems.Trading;

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
    [ValidateObjectMembers]
    public IntervalConfiguration Interval { get; set; } = new IntervalConfiguration();

    /// <summary>
    /// An amount of ETH to reserve for liquidity and gas fees.
    /// </summary>
    public decimal Reserve { get; set; } = 0.0M;

    /// <summary>
    /// The minimum amount of ETH or coins the <see cref="Trader"/> will attempt to trade.
    /// </summary>
    public decimal MinimumTrade { get; set; } = 0.00001M;

    /// <summary>
    /// A list of ERC20 tokens watched by the <see cref="Trader"/>.
    /// </summary>
    public Dictionary<string, string> Watchlist { get; set; } = [];

    /// <summary>
    /// Configurable options for the trader's trade interval.
    /// </summary>
    public class IntervalConfiguration
    {
        /// <summary>
        /// The interval's mode.
        /// </summary>
        public IntervalMode Mode { get; set; } = IntervalMode.Adaptive;

        /// <summary>
        /// The interval at which the trader trades.
        /// </summary>
        public TimeSpan Period { get; set; } = TimeSpan.FromHours(6);

        /// <summary>
        /// The interval's sensitivity to the market volatility.
        /// </summary>
        public double Sensitivity { get; set; } = 20.0D;
    }

    /// <summary>
    /// Determines the rate at which the <see cref="Trader"/> trades.
    /// </summary>
    public enum IntervalMode
    {
        /// <summary>
        /// The <see cref="Trader"/> executes trades on a fixed interval.
        /// </summary>
        Fixed,

        /// <summary>
        /// The <see cref="Trader"/> adapts its trade interval according to the market's volatility.
        /// </summary>
        Adaptive
    }
}