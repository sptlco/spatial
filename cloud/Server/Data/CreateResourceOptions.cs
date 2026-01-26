// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data;

/// <summary>
/// Configurable options for a new resource.
/// </summary>
public class CreateResourceOptions
{
    /// <summary>
    /// Configurable options for the resource.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = [];
}