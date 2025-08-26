// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Contracts.Systems.Trading;

namespace Spatial.Contracts.Systems;

/// <summary>
/// Configurable options for cloud systems.
/// </summary>
public class SystemConfiguration
{
    /// <summary>
    /// Configurable trading options.
    /// </summary>
    public TradeConfiguration Trading { get; set; } = new TradeConfiguration();
}
