// Copyright © Spatial Corporation. All rights reserved.

using Newtonsoft.Json;

namespace Spatial;

/// <summary>
/// Configurable options for the system.
/// </summary>
public class Configuration
{
    private static Configuration? _instance;

    /// <summary>
    /// Get the system's current <see cref="Configuration"/>.
    /// </summary>
    public static Configuration Current => GetOrBind();

    /// <summary>
    /// The system's current version.
    /// </summary>
    public string Version { get; set; }

    private static Configuration GetOrBind()
    {
        var path = Path.Join(AppDomain.CurrentDomain.BaseDirectory, Constants.ConfigurationPath);
        var json = File.ReadAllText(path);
 
        return _instance ??= JsonConvert.DeserializeObject<Configuration>(json)!;
    }
}