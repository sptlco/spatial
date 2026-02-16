// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Scopes;
using Spatial.Extensions;
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
    [Authorize(Scope.Assignments.List)]
    public async Task<List<Assignment>> ListAssignmentsAsync()
    {
        return Resource<Assignment>.List();
    }

    /// <summary>
    /// Patch a user's assignments.
    /// </summary>
    /// <param name="user">The user whose assignments to patch.</param>
    /// <param name="roles">A list of roles.</param>
    [PATCH]
    [Path("{user}")]
    [Authorize(Scope.Assignments.Patch)]
    public async Task PatchManyAsync(string user, [Body] List<string> roles)
    {
        Resource<Assignment>.RemoveMany(a => a.User == user);

        foreach (var role in roles)
        {
            var assignment = new Assignment {
                User = user,
                Role = role
            };

            assignment.Store();
        }
    }
}