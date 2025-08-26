// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Contracts.Systems;

namespace Spatial.Contracts;

/// <summary>
/// Configurable options for Spatial's cloud server.
/// </summary>
public class CloudConfiguration
{
    /// <summary>
    /// Configurable options for cloud systems.
    /// </summary>
    public SystemConfiguration Systems { get; set; } = new SystemConfiguration();
}