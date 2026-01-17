// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Principals;
using Spatial.Cloud.Data.Users;
using Spatial.Identity;
using Spatial.Identity.Authorization;
using System.Security.Claims;

namespace Spatial.Cloud.API;

/// <summary>
/// A controller for user functions.
/// </summary>
[Path("users")]
public class UserController : Controller
{
    /// <summary>
    /// Get the current user.
    /// </summary>
    /// <returns>The current user.</returns>
    [GET]
    [Path("me")]
    [Authorize]
    public async Task<User> GetUserAsync()
    {
        return new User {
            Account = _account,
            Session = _session,
            Principal = new Principal {
                Roles = GetClaims(ClaimsIdentity.DefaultRoleClaimType),
                Permissions = GetClaims(Claims.Permission)
            }
        };
    }

    /// <summary>
    /// Get a list of users.
    /// </summary>
    /// <returns>A list of users.</returns>
    [GET]
    [Path("list")]
    [Authorize]
    public async Task<List<User>> ListUsersAsync()
    {
        return [];
    }

    private List<string> GetClaims(string type)
    {
        return [.. HttpContext.User
            .FindAll(type)
            .Select(claim => claim.Value)
            .Distinct()];
    }
}