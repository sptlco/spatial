// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
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
    [ValidateObjectMembers]
    public TradeConfiguration Trading { get; set; } = new TradeConfiguration();
}
