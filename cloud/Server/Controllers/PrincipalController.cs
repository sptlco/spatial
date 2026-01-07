// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Principals;
using Spatial.Identity;
using Spatial.Identity.Authorization;
using System.Security.Claims;

namespace Spatial.Cloud.Controllers;

/// <summary>
/// A controller for the current user principal.
/// </summary>
[Path("principals")]
public class PrincipalController : Controller
{
    /// <summary>
    /// Get the current <see cref="Principal"/>.
    /// </summary>
    /// <returns>The current <see cref="Principal"/>.</returns>
    [GET]
    [Path("me")]
    [Authorize]
    public async Task<Principal> GetPrincipalAsync()
    {
        return await Task.FromResult(new Principal {
            Roles = [..HttpContext.User
                .FindAll(ClaimsIdentity.DefaultRoleClaimType)
                .Select(claim => claim.Value)
                .Distinct()],
            Permissions = [..HttpContext.User
                .FindAll(Claims.Permission)
                .Select(claim => claim.Value)
                .Distinct()]});
    }
}