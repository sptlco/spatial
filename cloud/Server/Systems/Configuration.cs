// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;

namespace Spatial.Cloud.Systems;

/// <summary>
/// Configurable options for systems.
/// </summary>
public class SystemConfiguration
{
    /// <summary>
    /// Configurable options for the <see cref="Systems.Hypersolver"/>.
    /// </summary>
    [ValidateObjectMembers]
    public HypersolverConfiguration Hypersolver { get; set; } = new HypersolverConfiguration();
}
