// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud;

/// <summary>
/// Configurable options for the <see cref="Server"/>.
/// </summary>
public class ServerConfiguration : Configuration
{
    /// <summary>
    /// Get the current <see cref="ServerConfiguration"/>.
    /// </summary>
    public new static ServerConfiguration Current => Server.Current.Configuration;
}