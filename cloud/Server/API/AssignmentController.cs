// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Assignments;
using Spatial.Extensions;
using Spatial.Identity.Authorization;

namespace Spatial.Cloud.API;

/// <summary>
/// A controller for role assignments.
/// </summary>
[Path("assignments")]
public class AssignmentController : Controller
{
    /// <summary>
    /// Create a new <see cref="Assignment"/>.
    /// </summary>
    /// <param name="options">Configurable options for the request.</param>
    /// <returns>An <see cref="Assignment"/>.</returns>
    [POST]
    [Authorize(Scope.Assignments.Create)]
    public async Task<Assignment> CreateAssignmentAsync([Body] CreateAssignmentOptions options)
    {
        var assignment = new Assignment {
            User = options.User,
            Role = options.Role
        };

        assignment.Store();

        return await Task.FromResult(assignment);
    }
}