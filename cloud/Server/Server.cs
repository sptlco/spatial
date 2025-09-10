// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts;

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