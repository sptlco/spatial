// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.IdentityModel.JsonWebTokens;
using Spatial.Helpers;
using Spatial.Identity.Authorization;
using Spatial.Persistence;
using System.Security.Claims;

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
        // Load the user's role assignments from the database.
        // For each assignment, load role metadata.

        var roles = Record<Assignment>
            .List(assignment => assignment.User == userId)
            .Select(assignment => Record<Role>.Read(assignment.Role));

        // For each role the user is assigned to, get the role's permissions.
        // Attach these to the user's authorization token.

        var permissions = roles
            .SelectMany(role => Record<Permission>
                .List(permission => permission.Role == role.Id)
                .Select(permission => permission.Value))
            .Distinct();

        var session = new Session {
            User = userId
        };

        session.Token = JWT.Create([
            new(JwtRegisteredClaimNames.Sid, session.Id),
            ..roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name)),
            ..permissions.Select(permission => new Claim(Claims.Permission, permission))
        ]);

        return session;
    }
}