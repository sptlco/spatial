// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Cloud.Models.Users;

/// <summary>
/// A unique identity known to the <see cref="Server"/>.
/// </summary>
[Collection("users")]
public class User : Record
{
    /// <summary>
    /// The user's email address.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The user's passphrase.
    /// </summary>
    public string Passphrase { get; set; }
}