// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Sessions;
using Spatial.Networking;

namespace Spatial.Cloud.Controllers;

/// <summary>
/// A <see cref="Controller"/> for sessions.
/// </summary>
[Module]
[Path("sessions")]
public class SessionController : Controller
{
    /// <summary>
    /// Create a new session.
    /// </summary>
    /// <param name="options">Configurable options sent to the server.</param>
    /// <returns>An authentication token.</returns>
    [POST]
    [Path("create")]
    public async Task<string> CreateSessionAsync([Body] CreateSessionOptions options)
    {
        // ...

        return await Task.FromResult(string.Empty);
    }
}