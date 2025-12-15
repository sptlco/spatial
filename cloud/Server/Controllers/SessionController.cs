// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Sessions;
using Spatial.Cloud.Models;
using Spatial.Cloud.Models.Users;
using Spatial.Extensions;
using Spatial.Identity;
using Spatial.Persistence;

namespace Spatial.Cloud.Controllers;

/// <summary>
/// A <see cref="Controller"/> for users.
/// </summary>
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
    public async Task CreateSessionAsync([Body] CreateSessionOptions options)
    {
        (Record<Key>.FirstOrDefault(key => 
            key.Owner == options.User && 
            key.Code.Equals(options.Key, StringComparison.CurrentCultureIgnoreCase) &&
            key.Expires > Time.Now) ?? throw new Unauthorized()).Remove();

        if (Record<Account>.FirstOrDefault(account => account.Email == options.User) is not Account account)
        {
            // This is the user's first time connecting.
            // We should create a new account, and send a welcome email.

            (account = new Account { Email = options.User }).Store();

            // ...
        }

        var session = Session.Create(account.Id);

        session.Agent = Request.Headers.UserAgent;

        session.Store();

        Response.Cookies.Append(
            Cookies.Token,
            session.Token,
            new CookieOptions {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.Add(Application.Current.Configuration.JWT.TTL)
            });

        await Task.CompletedTask;
    }
}