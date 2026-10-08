// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Helpers;
using Spatial.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Spatial.Identity;

/// <summary>
/// An active connection to an <see cref="Application"/>.
/// </summary>
[Collection("sessions")]
public class Session : Resource
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
    /// A short-lived access token (JWT).
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// A long-lived opauque refresh token.
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// The time the <see cref="Session"/> expires.
    /// </summary>
    public double Expires { get; set; }

    /// <summary>
    /// Create a new <see cref="Session"/>.
    /// </summary>
    /// <param name="userId">A user identifier.</param>
    /// <returns>A <see cref="Session"/> token.</returns>
    public static Session Create(string userId)
    {
        var session = new Session {
            User = userId,
        };

        session.Refresh();
        session.Regenerate();

        return session;
    }

    /// <summary>
    /// Refresh the <see cref="Session"/>.
    /// </summary>
    public void Refresh()
    {
        var expires = DateTime.UtcNow.Add(Application.Current.Configuration.JWT.RefreshTTL);

        Expires = Time.FromDateTime(expires);
        RefreshToken = JWT.Create(expires, new Claim(JwtRegisteredClaimNames.Sid, Id));
    }

    /// <summary>
    /// Mint a new short-lived access token bound to this <see cref="Session"/>.
    /// </summary>
    public void Regenerate()
    {
        AccessToken = JWT.Create(DateTime.UtcNow.Add(Application.Current.Configuration.JWT.TTL), new Claim(JwtRegisteredClaimNames.Sid, Id));
    }
}