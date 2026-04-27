// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Cloud.ECS;

namespace Spatial.Cloud;

/// <summary>
/// Configurable options for the <see cref="Server"/>.
/// </summary>
internal class ServerConfiguration : Configuration
{
    /// <summary>
    /// Get the current <see cref="ServerConfiguration"/>.
    /// </summary>
    public new static ServerConfiguration Current => Server.Current.Configuration;

    /// <summary>
    /// Configurable options for Baymax.
    /// </summary>
    [ValidateObjectMembers]
    public BaymaxConfiguration Baymax { get; set; } = new BaymaxConfiguration();
}