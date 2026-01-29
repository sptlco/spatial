// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Authorization;
using Spatial.Cloud.Data.Accounts;
using Spatial.Cloud.Data.Keys;
using Spatial.Cloud.Data.Sessions;
using Spatial.Communication;
using Spatial.Extensions;
using Spatial.Identity;
using Spatial.Persistence;

namespace Spatial.Cloud.API;

/// <summary>
/// A <see cref="Controller"/> for users.
/// </summary>
[Path("sessions")]
public class SessionController : Controller
{
    /// <summary>
    /// Get the current session.
    /// </summary>
    /// <returns>The current session.</returns>
    [GET]
    [Path("me")]
    [Authorize]
    public async Task<Session> GetSessionAsync()
    {
        return _session;    
    }

    /// <summary>
    /// Create a new <see cref="Session"/>.
    /// </summary>
    /// <param name="options">Configurable options for the request.</param>
    /// <returns>A <see cref="Session"/>.</returns>
    [POST]
    public async Task<Session> CreateSessionAsync([Body] CreateSessionOptions options)
    {
        (Resource<Key>.FirstOrDefault(key => 
            key.Owner == options.User && 
            key.Code.Equals(options.Key, StringComparison.CurrentCultureIgnoreCase) &&
            key.Expires > Time.Now) ?? throw new Unauthorized()).Remove();

        var account = Resource<Account>.First(account => account.Email == options.User);
        var session = Session.Create(account.Id);

        session.Agent = Request.Headers.UserAgent;

        session.Store();

        return await Task.FromResult(session);
    }

    /// <summary>
    /// Destroy the current session.
    /// </summary>
    /// <returns></returns>
    [DELETE]
    [Path("me")]
    [Authorize]
    public async Task DestroySessionAsync()
    {
        _session.Remove();
    }
}