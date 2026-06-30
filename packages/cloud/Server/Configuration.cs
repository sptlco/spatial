// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Services;

namespace Spatial.Cloud;

/// <summary>
/// Configurable options for the <see cref="Server"/>.
/// </summary>
public class ServerConfiguration : Configuration
{
    /// <summary>
    /// Configurable options for the <see cref="Services.Allocator"/>.
    /// </summary>
    public AllocatorConfiguration Allocator { get; set; } = new AllocatorConfiguration();
}