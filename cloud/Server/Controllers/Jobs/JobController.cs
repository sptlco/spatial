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
    /// Create a new <see cref="GraphController"/>.
    /// </summary>
    public JobController()
    {
        _compiler = new Compiler();
    }
    
    /// <summary>
    /// Create a new <see cref="Job"/>.
    /// </summary>
    /// <remarks>Under the hood, the request will be forward to the graph endpoint.</remarks>
    /// <param name="instructions"><see cref="Instructions"/> for the <see cref="Server"/>.</param>
    /// <returns>A <see cref="Job"/>.</returns>
    [POST]
    [Path("/")]
    public async Task<Job> CreateJobAsync([Body] Instructions instructions) => (await CreateGraphAsync([instructions])).Jobs.First();
    
    /// <summary>
    /// Create a new <see cref="Graph"/>.
    /// </summary>
    /// <param name="jobs"><see cref="Instructions"/> for each <see cref="Job"/> in the <see cref="Graph"/>.</param>
    /// <returns>A <see cref="Graph"/>.</returns>
    [POST]
    [Path("/graph")]
    public async Task<Graph> CreateGraphAsync([Body] List<Instructions> jobs)
    {
        var graph = _compiler.Compile();

        // ...

        return await Task.FromResult(new Graph {
            // ...
        });
    }
}