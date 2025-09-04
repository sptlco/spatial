// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;

namespace Spatial.Contracts.Systems;

/// <summary>
/// Configurable options for cloud systems.
/// </summary>
public class SystemConfiguration
{
    /// <summary>
    /// Configurable options for the <see cref="Systems.Trader"/>.
    /// </summary>
    [ValidateObjectMembers]
    public TraderConfiguration Trader { get; set; } = new TraderConfiguration();
}