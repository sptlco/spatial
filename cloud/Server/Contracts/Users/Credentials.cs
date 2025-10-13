// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Contracts.Users;

/// <summary>
/// ...
/// </summary>
public class Credentials
{
    /// <summary>
    /// A string traceable to a unique <see cref="User"/>.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// A cryptographically secured passphrase.
    /// </summary>
    public string Passphrase { get; set; }
}