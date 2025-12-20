// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Cloud.Baymax;

namespace Spatial.Cloud;

/// <summary>
/// Configurable options for the <see cref="Server"/>.
/// </summary>
internal class ServerConfiguration : Configuration
{
    /// <summary>
    /// Get the current <see cref="ServerConfiguration"/>.
    /// </summary>
    public new static ServerConfiguration Current => Application.Current.Services.GetRequiredService<ServerConfiguration>();

    /// <summary>
    /// Configurable options for Baymax.
    /// </summary>
    [ValidateObjectMembers]
    public BaymaxConfiguration Baymax { get; set; } = new BaymaxConfiguration();
}