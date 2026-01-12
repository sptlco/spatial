// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Cloud.ECS.Systems;

namespace Spatial.Cloud.ECS;

/// <summary>
/// Configurable options for Baymax.
/// </summary>
public class BaymaxConfiguration
{
    /// <summary>
    /// Configurable options for the <see cref="Systems.Hypersolver"/>.
    /// </summary>
    [ValidateObjectMembers]
    public HypersolverConfiguration Hypersolver { get; set; } = new HypersolverConfiguration();
}
