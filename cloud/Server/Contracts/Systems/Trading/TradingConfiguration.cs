// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;

namespace Spatial.Cloud.Contracts.Systems.Trading;

/// <summary>
/// Configurable options for trading.
/// </summary>
internal class TradingConfiguration
{
    /// <summary>
    /// Configurable options for the <see cref="Banking.Trader"/>.
    /// </summary>
    [ValidateObjectMembers]
    public TraderConfiguration Trader { get; set; } = new TraderConfiguration();
}