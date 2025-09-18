// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Cloud.Contracts.Systems;

namespace Spatial.Cloud.Contracts;

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
    /// Configurable options for cloud systems.
    /// </summary>
    [ValidateObjectMembers]
    public SystemConfiguration Systems { get; set; } = new SystemConfiguration();
}