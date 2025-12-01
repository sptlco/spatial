// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Cloud.Models;

/// <summary>
/// 
/// </summary>
public abstract class Resource : Record
{
    /// <summary>
    /// ...
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = [];
}