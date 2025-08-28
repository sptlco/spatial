// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Contracts;

namespace Spatial;

/// <summary>
/// An autonomous <see cref="Application"/> that lives in the cloud.
/// </summary>
public class Server : Application
{
    /// <summary>
    /// Configure the <see cref="Server"/>.
    /// </summary>
    /// <param name="builder">The server's <see cref="IHostApplicationBuilder"/>.</param>
    public override void Configure(IHostApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<CloudConfiguration>()
            .Bind(builder.Configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}