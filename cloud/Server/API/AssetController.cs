// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Assets;
using Spatial.Cloud.Data.Scopes;
using Spatial.Identity.Authorization;
using Spatial.Persistence;

namespace Spatial.Cloud.API;

/// <summary>
/// A <see cref="Controller"/> for <see cref="Asset"/> functions.
/// </summary>
[Path("assets")]
public class AssetController : Controller
{
    /// <summary>
    /// Get a list of assets.
    /// </summary>
    /// <returns>A list of assets.</returns>
    [GET]
    [Authorize(Scope.Assets.List)]
    public Task<List<Asset>> ListAssetsAsync()
    {
        return Resource<Asset>.ListAsync();
    }
}