// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Accounts;
using Spatial.Cloud.Data.Principals;
using Spatial.Cloud.Data.Scopes;
using Spatial.Cloud.Data.Users;
using Spatial.Identity;
using Spatial.Identity.Authorization;
using Spatial.Persistence;
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
    [Path("current")]
    [Authorize]
    public async Task<User> CurrentUserAsync()
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
    //[Authorize(Scope.Users.List)]
    public async Task<List<User>> ListUsersAsync()
    {
        var users = Resource<Account>
            .List()
            .Select(account => {
                var roles = Resource<Assignment>
                    .List(a => a.User == account.Id)
                    .Select(a => Resource<Role>.Read(a.Role));

                var permissions = roles
                    .SelectMany(r => Resource<Permission>.List(p => p.Role == r.Id))
                    .Select(p => p.Scope)
                    .Distinct();
                
                return new User {
                    Account = account,
                    Principal = new Principal { 
                        Roles = [..roles.Select(role => role.Name)],
                        Permissions = [..permissions]
                    }
                };
            });

        return [..users];
    }

    private List<string> GetClaims(string type)
    {
        return [.. HttpContext.User
            .FindAll(type)
            .Select(claim => claim.Value)
            .Distinct()];
    }
}