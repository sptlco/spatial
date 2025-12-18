// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Keys;

/// <summary>
/// Configurable options for a new key.
/// </summary>
public class CreateKeyOptions
{
    /// <summary>
    /// The account the <see cref="Keys.Key"/> provides access to.
    /// </summary>
    public string User { get; set; }
}