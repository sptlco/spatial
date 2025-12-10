// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Sessions;
using Spatial.Cloud.Models;
using Spatial.Extensions;
using Spatial.Identity;
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
    public Task<string> CreateSessionAsync([Body] CreateSessionOptions options)
    {
        var key = Record<Key>.FirstOrDefault(key => 
            key.Owner == options.User && 
            key.Code.Equals(options.Key, StringComparison.CurrentCultureIgnoreCase) &&
            key.Expires > Time.Now) ?? throw new Unauthorized();

        key.Remove();

        var account = Record<Account>.FirstOrDefault(account => account.Email == options.User);

        if (account is null)
        {
            // This is the user's first time connecting.
            // ...

            account = new Account { Email = options.User };

            account.Store();

            // ...
        }

        // ...

        return Task.FromResult(Token.Create(account.Id, account.Email));
    }
}