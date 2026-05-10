// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Cloud.Data.Transmissions;

/// <summary>
/// A secure message.
/// </summary>
[Collection("transmissions")]
public class Transmission : Resource
{
    /// <summary>
    /// The transmission's secure passphrase hash.
    /// </summary>
    public string Passphrase { get; set; }

    /// <summary>
    /// The transmission's relative URL path.
    /// </summary>
    public string Path { get; set; }
}