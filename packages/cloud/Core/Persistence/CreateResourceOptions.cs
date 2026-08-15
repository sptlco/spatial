// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Persistence;

/// <summary>
/// Configurable options for a new <see cref="Resource"/>.
/// </summary>
public class CreateResourceOptions
{
    /// <summary>
    /// Configurable options for the <see cref="Resource"/>.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = [];
}