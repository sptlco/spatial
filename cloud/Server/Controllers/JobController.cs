// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Jobs;
using Spatial.Compute.Commands;
using Spatial.Networking;

namespace Spatial.Cloud.Controllers;

/// <summary>
/// <see cref="Controller"/> for compute jobs.
/// </summary>
[Module]
[Path("jobs")]
public class JobController : Controller
{
    /// <summary>
    /// Schedule a <see cref="Job"/>.
    /// </summary>
    /// <param name="instructions">Instructions for the job's execution.</param>
    /// <returns>A <see cref="Job"/>.</returns>
    [POST]
    [Path("schedule")]
    public async Task ScheduleJobAsync([Json] Instructions instructions)
    {
        await Compute.Job.ScheduleAsync(CreateJob(instructions));
    }

    private CommandJob CreateJob(Instructions instructions)
    {
        return new CommandJob(() => INFO("Hello, world!"));
    }
}