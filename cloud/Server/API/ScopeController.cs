// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Scopes;
using Spatial.Identity.Authorization;

namespace Spatial.Cloud.API;

/// <summary>
/// A <see cref="Controller"/> for scope functions.
/// </summary>
[Path("scopes")]
public class ScopeController : Controller
{
    /// <summary>
    /// Get a list of scopes.
    /// </summary>
    /// <returns>A list of scopes.</returns>
    [GET]
    [Path("list")]
    //[Authorize(Scope.Scopes.List)]
    public async Task<List<Sector>> ListScopesAsync()
    {
        return Server.Current.Scopes;
    }
}