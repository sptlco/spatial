// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Sessions;
using Spatial.Cloud.Models;
using Spatial.Extensions;
using Spatial.Networking;
using Spatial.Persistence;

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
        var key = Record<Key>.FirstOrDefault(key => 
            key.Owner == options.UID && 
            key.Code == options.Key &&
            key.Expires > Time.Now);

        if (key is null)
        {
            throw new Unauthorized();
        }

        key.Remove();

        var account = Record<Account>.FirstOrDefault(account => account.Email == options.UID);

        if (account is null)
        {
            // This is the user's first time connecting.
            // ...

            account = new Account { Email = options.UID };

            account.Save();

            // ...
        }

        return await Task.FromResult(string.Empty);
    }
}