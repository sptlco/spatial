// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Networking;
using Spatial.Persistence;

namespace Spatial;

/// <summary>
/// Configurable options for the system.
/// </summary>
public class Configuration
{
    /// <summary>
    /// Get the current <see cref="Configuration"/>.
    /// </summary>
    public static Configuration Current => Application.Current.Configuration;

    /// <summary>
    /// The system's version.
    /// </summary>
    public string Version { get; set; } = "";

    /// <summary>
    /// Configurable options for the system's database.
    /// </summary>
    public DatabaseConfiguration Database { get; set; } = new DatabaseConfiguration();

    /// <summary>
    /// Configurable options for the system's private network.
    /// </summary>
    public NetworkConfiguration Network { get; set; } = new NetworkConfiguration();
}