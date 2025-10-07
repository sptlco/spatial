// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts;
using Spatial.Networking;

namespace Spatial.Cloud.Controllers;

/// <summary>
/// A <see cref="Controller"/> for <see cref="Compute.Graph"/>.
/// </summary>
[Module]
[Path("graphs")]
public class GraphController : Controller
{
    /// <summary>
    /// Create a new <see cref="Graph"/>.
    /// </summary>
    /// <returns>A <see cref="Graph"/>.</returns>
    [POST]
    [Path("/")]
    public async Task<Graph> CreateGraphAsync()
    {
        // ...

        return await Task.FromResult(new Graph());
    }
}