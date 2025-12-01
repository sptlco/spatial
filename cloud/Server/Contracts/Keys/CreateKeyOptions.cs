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
}