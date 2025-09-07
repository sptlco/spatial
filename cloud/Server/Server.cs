// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Cloud.Systems;

namespace Spatial.Cloud;

/// <summary>
/// An autonomous <see cref="Application"/> that lives in the cloud.
/// </summary>
internal class Server : Application
{
    /// <summary>
    /// Configure the <see cref="Server"/>.
    /// </summary>
    /// <param name="builder">The server's <see cref="IHostApplicationBuilder"/>.</param>
    public override void Configure(IHostApplicationBuilder builder)
    {
        AddOptions<ServerConfiguration>(builder);
    }
}

/// <summary>
/// Configurable options for the <see cref="Server"/>.
/// </summary>
internal class ServerConfiguration
{
    /// <summary>
    /// Configurable options for cloud systems.
    /// </summary>
    [ValidateObjectMembers]
    public SystemConfiguration Systems { get; set; } = new SystemConfiguration();
}