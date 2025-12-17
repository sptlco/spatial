// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Roles;
using Spatial.Extensions;
using Spatial.Identity.Authorization;

namespace Spatial.Cloud.Controllers;

/// <summary>
/// A controller for roles.
/// </summary>
[Path("roles")]
public class RoleController : Controller
{
    /// <summary>
    /// Create a new <see cref="Role"/>.
    /// </summary>
    /// <param name="options">Configurable options for the request.</param>
    /// <returns>A <see cref="Role"/>.</returns>
    [POST]
    [Authorize(Scope.Roles.Create)]
    public async Task<Role> CreateRoleAsync([Body] CreateRoleOptions options)
    {
        var role = new Role {
            Name = options.Name,
            Description = options.Description
        };

        role.Store();

        return await Task.FromResult(role);
    }
}