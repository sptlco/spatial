// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Users;
using Spatial.Networking;

namespace Spatial.Cloud.Controllers;

/// <summary>
/// A <see cref="Controller"/> for <see cref="Session"/>.
/// </summary>
[Module]
[Path("sessions")]
public class SessionController : Controller
{
    /// <summary>
    /// Create a new <see cref="Session"/>.
    /// </summary>
    /// <param name="credentials">The user's <see cref="Credentials"/>.</param>
    /// <returns>An authentication token.</returns>
    [POST]
    [Path("/")]
    public async Task<string> CreateSessionAsync([Body] Credentials credentials)
    {
        // ...

        return await Task.FromResult(string.Empty);
    }
}