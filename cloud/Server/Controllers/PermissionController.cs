// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Permissions;
using Spatial.Extensions;
using Spatial.Identity.Authorization;

namespace Spatial.Cloud.Controllers;

/// <summary>
/// A controller for permissions.
/// </summary>
[Path("permissions")]
public class PermissionController : Controller
{
    /// <summary>
    /// Create a new <see cref="Permission"/>.
    /// </summary>
    /// <param name="options">Configurable options for the request.</param>
    /// <returns>A <see cref="Permission"/>.</returns>
    [POST]
    [Authorize(Scope.Permissions.Create)]
    public async Task<Permission> CreatePermissionAsync([Body] CreatePermissionOptions options)
    {
        var permission = new Permission {
            Role = options.Role,
            Scope = options.Scope
        };

        permission.Store();

        return await Task.FromResult(permission);
    }
}