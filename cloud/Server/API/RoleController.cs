// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Roles;
using Spatial.Extensions;
using Spatial.Identity.Authorization;
using Spatial.Persistence;

namespace Spatial.Cloud.API;

/// <summary>
/// A controller for roles.
/// </summary>
[Path("roles")]
public class RoleController : Controller
{
    /// <summary>
    /// Create a new <see cref="Role"/>.
    /// </summary>
    /// <param name="options">Configurable options for the <see cref="Role"/>.</param>
    /// <returns>A <see cref="Role"/>.</returns>
    [POST]
    [Authorize]
    public async Task<Role> CreateRoleAsync([Body] CreateRoleOptions options)
    {
        var role = new Role {
            Name = options.Name,
            Description = options.Description,
            Color = options.Color,
            Metadata = options.Metadata
        };

        role.Store();

        return role;
    }

    /// <summary>
    /// Update a <see cref="Role"/>.
    /// </summary>
    /// <param name="role">The <see cref="Role"/> to update.</param>
    /// <returns>The updated <see cref="Role"/>.</returns>
    [PATCH]
    [Authorize]
    public async Task<Role> UpdateRoleAsync([Body] Role role)
    {
        role.Save();

        return role;
    }

    /// <summary>
    /// Get a list of roles.
    /// </summary>
    /// <returns>A list of roles.</returns>
    [GET]
    [Path("list")]
    [Authorize]
    public async Task<List<Role>> ListRolesAsync()
    {
        return Resource<Role>.List();
    }

    /// <summary>
    /// Delete a <see cref="Role"/>.
    /// </summary>
    /// <param name="id">The role to delete.</param>
    [DELETE]
    [Path("{id}")]
    [Authorize]
    public async Task DeleteRoleAsync(string id)
    {
        Resource<Role>.RemoveOne(role => role.Id == id);
    }
}