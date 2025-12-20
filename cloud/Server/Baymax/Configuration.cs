// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Cloud.Baymax.Systems;

namespace Spatial.Cloud.Baymax;

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
