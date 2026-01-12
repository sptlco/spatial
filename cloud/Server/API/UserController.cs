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
                Roles = [..HttpContext.User
                    .FindAll(ClaimsIdentity.DefaultRoleClaimType)
                    .Select(claim => claim.Value)
                    .Distinct()],
                Permissions = [..HttpContext.User
                    .FindAll(Claims.Permission)
                    .Select(claim => claim.Value)
                    .Distinct()]}};
    }
}