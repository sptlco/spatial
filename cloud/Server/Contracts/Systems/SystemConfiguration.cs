// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Cloud.Contracts.Systems.Trading;

namespace Spatial.Cloud.Contracts.Systems;

/// <summary>
/// Configurable options for cloud systems.
/// </summary>
internal class SystemConfiguration
{
    /// <summary>
    /// Configurable options for trading.
    /// </summary>
    [ValidateObjectMembers]
    public TradingConfiguration Trading { get; set; } = new TradingConfiguration();
}