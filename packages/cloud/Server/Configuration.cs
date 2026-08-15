// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
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
    [ValidateObjectMembers]
    public AllocatorConfiguration Allocator { get; set; } = new AllocatorConfiguration();
}