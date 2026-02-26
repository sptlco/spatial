// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Accounts;
using Spatial.Cloud.Data.Keys;
using Spatial.Extensions;
using Spatial.Helpers;
using Spatial.Persistence;

namespace Spatial.Cloud.API;

/// <summary>
/// A <see cref="Controller"/> for keys.
/// </summary>
[Path("keys")]
public class KeyController : Controller
{
    /// <summary>
    /// Create a new key.
    /// </summary>
    /// <param name="options">Configurable options for the key.</param>
    /// <returns>A key identifier.</returns>
    [POST]
    public Task CreateKeyAsync([Body] CreateKeyOptions options)
    {
        if (Resource<Account>.FirstOrDefault(a => a.Email == options.User) is not Account account)
        {
            throw new UserError("The requested user does not exist.");
        }

        var key = new Key { Owner = account.Email };

        key.Store();

        Smtp.Send(
            subject: $"Your key code is {key.Code}", 
            body: key.Code,
            recipients: key.Owner);

        return Task.CompletedTask;
    }
}