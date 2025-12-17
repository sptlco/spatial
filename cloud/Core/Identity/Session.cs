// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.IdentityModel.JsonWebTokens;
using Spatial.Helpers;
using Spatial.Persistence;

namespace Spatial.Identity;

/// <summary>
/// An active connection to an <see cref="Application"/>.
/// </summary>
[Collection("sessions")]
public class Session : Record
{
    /// <summary>
    /// A user identifier.
    /// </summary>
    public string User { get; set; }

    /// <summary>
    /// The device the user is connecting from.
    /// </summary>
    public string? Agent { get; set; }

    /// <summary>
    /// The session's authorization token.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Create a new <see cref="Session"/>.
    /// </summary>
    /// <param name="userId">A user identifier.</param>
    /// <returns>A <see cref="Session"/> token.</returns>
    public static Session Create(string userId)
    {
        var session = new Session {
            User = userId
        };

        session.Token = JWT.Create([new(JwtRegisteredClaimNames.Sid, session.Id)]);

        return session;
    }
}