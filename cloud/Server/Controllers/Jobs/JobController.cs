// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Jobs;
using Spatial.Networking;

namespace Spatial.Cloud.Controllers.Jobs;

/// <summary>
/// A <see cref="Controller"/> for <see cref="Job"/>.
/// </summary>
[Module]
[Path("jobs")]
public class JobController : Controller
{
    /// <summary>
    /// Create a new <see cref="Job"/>.
    /// </summary>
    /// <returns>A <see cref="Job"/>.</returns>
    [POST]
    [Path("/")]
    public async Task<Job> CreateJobAsync()
    {
        // ...

        return await Task.FromResult(new Job());
    }

    /// <summary>
    /// Create a new <see cref="Graph"/>.
    /// </summary>
    /// <returns>A <see cref="Graph"/>.</returns>
    [POST]
    [Path("/graph")]
    public async Task<Graph> CreateGraphAsync()
    {
        // ...

        return await Task.FromResult(new Graph());
    }   
}