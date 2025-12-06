// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Contracts.Keys;

/// <summary>
/// Configurable options for a new key.
/// </summary>
public class CreateKeyOptions
{
    /// <summary>
    /// A <see cref="Models.Resource"/> identifier.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// The number of milliseconds the <see cref="Models.Key"/> is valid for.
    /// </summary>
    public double TTL { get; set; }

    /// <summary>
    /// The reason the <see cref="Models.Key"/> is being created.
    /// </summary>
    public string Reason { get; set; }
}