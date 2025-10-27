// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Cloud.Systems;
using System.Reflection;

namespace Spatial.Cloud;

/// <summary>
/// An autonomous <see cref="Application"/> that lives in the cloud.
/// </summary>
internal class Server : Application
{
    private readonly Dictionary<int, IActuator> _actuators;

    /// <summary>
    /// Create a new <see cref="Server"/>.
    /// </summary>
    public Server()
    {
        _actuators = Assembly
            .GetEntryAssembly()!
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IActuator)) && type.GetCustomAttribute<ActuatorAttribute>() != default)
            .ToDictionary(
                keySelector: type => type.GetCustomAttribute<ActuatorAttribute>()!.Id,
                elementSelector: type => (IActuator) Activator.CreateInstance(type)!);
    }

    /// <summary>
    /// Get the current <see cref="Server"/>.
    /// </summary>
    public static new Server Current => (Server) Application.Current;

    /// <summary>
    /// The server's feature <see cref="Systems.Extractor"/>.
    /// </summary>
    public Extractor Extractor { get; internal set; }

    /// <summary>
    /// The server's <see cref="Systems.Hypersolver"/>.
    /// </summary>
    public Hypersolver Hypersolver { get; internal set; }

    /// <summary>
    /// A list of actuators.
    /// </summary>
    public Dictionary<int, IActuator> Actuators => _actuators;

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
internal class ServerConfiguration : Configuration
{
    /// <summary>
    /// Get the current <see cref="ServerConfiguration"/>.
    /// </summary>
    public new static ServerConfiguration Current => Application.Current.Services.GetRequiredService<ServerConfiguration>();

    /// <summary>
    /// Configurable options for systems.
    /// </summary>
    [ValidateObjectMembers]
    public SystemConfiguration Systems { get; set; } = new SystemConfiguration();
}