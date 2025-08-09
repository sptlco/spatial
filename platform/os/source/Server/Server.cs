// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Contracts;
using Spatial.Systems.Computing;
using Spatial.Systems.Tokens.Swapping;

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
            .Bind(builder.Configuration.GetSection(Constants.CloudSettingSection))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    /// <summary>
    /// Start the <see cref="Server"/>.
    /// </summary>
    public override void Start()
    {
        Use<Computer>();
        Use<Swapper>();
    }
}