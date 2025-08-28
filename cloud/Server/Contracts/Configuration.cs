// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Contracts.Systems;

namespace Spatial.Contracts;

/// <summary>
/// Configurable options for the server.
/// </summary>
public class CloudConfiguration
{
    /// <summary>
    /// Configurable options for cloud systems.
    /// </summary>
    [ValidateObjectMembers]
    public SystemConfiguration Systems { get; set; } = new SystemConfiguration();
}