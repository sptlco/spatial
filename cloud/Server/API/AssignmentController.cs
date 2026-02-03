// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Identity.Authorization;
using Spatial.Persistence;

namespace Spatial.Cloud.API;

/// <summary>
/// A <see cref="Controller"/> for assignments.
/// </summary>
[Path("assignments")]
public class AssignmentController : Controller
{
    /// <summary>
    /// Get a list of assignments.
    /// </summary>
    /// <returns>A list of assignments.</returns>
    [GET]
    [Path("list")]
    [Authorize]
    public async Task<List<Assignment>> ListAssignmentsAsync()
    {
        return Resource<Assignment>.List();
    }
}