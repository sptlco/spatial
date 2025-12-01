// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Sessions;
using Spatial.Networking;

namespace Spatial.Cloud.Controllers;

/// <summary>
/// A <see cref="Controller"/> for users.
/// </summary>
[Module]
[Path("sessions")]
public class SessionController : Controller
{
    /// <summary>
    /// Create a new session.
    /// </summary>
    /// <param name="options">Configurable options for the session.</param>
    /// <returns>A session identifier.</returns>
    [POST]
    [Path("create")]
    public async Task<string> CreateSessionAsync([Body] CreateSessionOptions options)
    {
        // ...

        return await Task.FromResult(string.Empty);
    }
}