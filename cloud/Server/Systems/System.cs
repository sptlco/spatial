// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Simulation;

namespace Spatial.Cloud.Systems;

/// <summary>
/// A fixture that defines generic behavior.
/// </summary>
internal abstract class System : System<Space> { }

/// <summary>
/// Configurable options for cloud systems.
/// </summary>
internal class SystemConfiguration
{
    /// <summary>
    /// Configurable options for the <see cref="Systems.Trader"/>.
    /// </summary>
    [ValidateObjectMembers]
    public TraderConfiguration Trader { get; set; } = new TraderConfiguration();
}