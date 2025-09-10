// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Cloud.Contracts.Systems;

namespace Spatial.Cloud.Contracts;

/// <summary>
/// Configurable options for the <see cref="Server"/>.
/// </summary>
internal class ServerConfiguration
{
    /// <summary>
    /// Get the current <see cref="ServerConfiguration"/>.
    /// </summary>
    public static ServerConfiguration Current => Application.Current.Services.GetRequiredService<ServerConfiguration>();

    /// <summary>
    /// Configurable options for OpenAI.
    /// </summary>
    [ValidateObjectMembers]
    public OpenAIConfiguration OpenAI { get; set; } = new OpenAIConfiguration();
    
    /// <summary>
    /// Configurable options for cloud systems.
    /// </summary>
    [ValidateObjectMembers]
    public SystemConfiguration Systems { get; set; } = new SystemConfiguration();
}