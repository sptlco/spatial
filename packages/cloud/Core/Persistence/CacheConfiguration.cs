// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Persistence;

/// <summary>
/// Configurable options for <see cref="Cache"/>.
/// </summary>
public class CacheConfiguration
{
    /// <summary>
    /// A Redis database URL.
    /// </summary>
    public string Url { get; set; } = "redis:6379,abortConnect=false";

    /// <summary>
    /// A Redis database identification number.
    /// </summary>
    public int Database { get; set; } = -1;
}