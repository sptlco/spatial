// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Users;
using Spatial.Networking;

namespace Spatial.Cloud.Controllers.Users;

/// <summary>
/// A controller for <see cref="User"/>.
/// </summary>
[Module]
[Path("users")]
public class UserController : Controller
{
    /// <summary>
    /// Create a new <see cref="User"/>.
    /// </summary>
    /// <param name="credentials">The user's <see cref="Credentials"/>.</param>
    /// <returns>A <see cref="User"/>.</returns>
    [POST]
    [Path("/")]
    public async Task<User> CreateUserAsync([Body] Credentials credentials)
    {
        // ...

        return await Task.FromResult(new User());
    }
}