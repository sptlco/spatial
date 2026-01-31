// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data;
using Spatial.Cloud.Data.Permissions;
using Spatial.Cloud.Data.Scopes;
using Spatial.Extensions;
using Spatial.Identity.Authorization;
using Spatial.Persistence;

namespace Spatial.Cloud.API;

/// <summary>
/// A controller for permissions.
/// </summary>
[Path("permissions")]
public class PermissionController : Controller
{
    [PATCH]
    //[Authorize(Scope.Permissions.Update)]
    public async Task UpdatePermissionsAsync([Body] Difference<PermissionSlim> diff)
    {
        diff.Added.ForEach(p => new Permission { Role = p.Role, Scope = p.Scope }.Store());
        Resource<Permission>.RemoveMany(a => diff.Removed.Exists(b => b.Role == a.Role && b.Scope == a.Scope));
    }

    /// <summary>
    /// List all permissions.
    /// </summary>
    /// <returns>A list of permissions.</returns>
    [GET]
    [Path("list")]
    //[Authorize(Scope.Permissions.List)]
    public async Task<List<Permission>> ListPermissionsAsync()
    {
        return Resource<Permission>.List();
    }
}