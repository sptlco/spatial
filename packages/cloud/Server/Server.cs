// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Cloud.Services;

namespace Spatial.Cloud;

/// <summary>
/// An autonomous <see cref="Application"/> that lives in the cloud.
/// </summary>
public class Server : Application
{
    /// <summary>
    /// Get the current <see cref="Server"/>.
    /// </summary>
    public static new Server Current => (Server) Application.Current;

    /// <summary>
	/// The server's current <see cref="ServerConfiguration"/>.
	/// </summary>
	public new ServerConfiguration Configuration => Services.GetRequiredService<IOptionsMonitor<ServerConfiguration>>().CurrentValue;

    /// <summary>
    /// Configure the server's <see cref="IHostApplicationBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IHostApplicationBuilder"/> to configure.</param>
    public override void ConfigureBuilder(IHostApplicationBuilder builder)
    {
        var configuration = builder.Configuration.Get<ServerConfiguration>();

        AddOptions<ServerConfiguration>(builder);

        if (configuration?.Allocator is { Enabled: true })
        {
            builder.Services.AddHostedService<Allocator>();
        }
    }
}