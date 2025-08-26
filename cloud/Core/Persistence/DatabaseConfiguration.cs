// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Persistence;

/// <summary>
/// Configurable options for the system's database.
/// </summary>
public class DatabaseConfiguration
{
    /// <summary>
    /// The database's URL.
    /// </summary>
    public string Url { get; set; } = Constants.DefaultDatabaseUrl;

    /// <summary>
    /// The database's name.
    /// </summary>
    public string Name { get; set; } = Constants.DefaultDatabaseName;

    /// <summary>
    /// The database's connection string.
    /// </summary>
    public string ConnectionString => string.Join('/', Url, Name);
}
