// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Jobs;
using Spatial.Cloud.Services.Jobs;
using Spatial.Networking;

namespace Spatial.Cloud.Controllers.Jobs;

/// <summary>
/// A <see cref="Controller"/> for <see cref="Job"/>.
/// </summary>
[Module]
[Path("jobs")]
public class JobController : Controller
{
    private readonly Compiler _compiler;

    /// <summary>
    /// Create a new <see cref="JobController"/>.
    /// </summary>
    public JobController()
    {
        _compiler = new Compiler();
    }

    /// <summary>
    /// Create a new <see cref="Job"/>.
    /// </summary>
    /// <remarks>Once created, the job is automatically scheduled by the <see cref="Server"/>.</remarks>
    /// <returns>A <see cref="Job"/>.</returns>
    [POST]
    [Path("/")]
    public async Task<Job> CreateJobAsync()
    {
        var graph = _compiler.Compile();

        // ...

        return await Task.FromResult(new Job {
            // ...
        });
    }

    /// <summary>
    /// Create a new <see cref="Graph"/>.
    /// </summary>
    /// <remarks>Once created, the job is automatically scheduled by the <see cref="Server"/>.</remarks>
    /// <returns>A <see cref="Graph"/>.</returns>
    [POST]
    [Path("/graph")]
    public async Task<Graph> CreateGraphAsync()
    {
        var graph = _compiler.Compile();

        // ...

        return await Task.FromResult(new Graph {
            // ...
        });
    }   
}