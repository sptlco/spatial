// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts;
using Spatial.Networking;

namespace Spatial.Cloud.Controllers;

/// <summary>
/// A <see cref="Controller"/> for <see cref="Compute.Job"/>.
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
}