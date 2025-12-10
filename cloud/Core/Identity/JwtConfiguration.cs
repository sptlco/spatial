// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Identity;

/// <summary>
/// Configurable options for JWT.
/// </summary>
public class JwtConfiguration
{
    /// <summary>
    /// The entity issuing authentication tokens.
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// The recipients of authentication tokens.
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// A secret value used for authentication purposes.
    /// </summary>
    public string Secret { get; set; }

    /// <summary>
    /// Determines how long an authentication token lasts for.
    /// </summary>
    public TimeSpan TTL { get; set; }
}