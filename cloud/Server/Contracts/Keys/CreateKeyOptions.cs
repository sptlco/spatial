// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Contracts.Keys;

/// <summary>
/// Configurable options for a new key.
/// </summary>
public class CreateKeyOptions
{
    /// <summary>
    /// The account the <see cref="Models.Key"/> provides access to.
    /// </summary>
    public string UID { get; set; }
}