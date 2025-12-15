// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Keys;
using Spatial.Cloud.Models;
using Spatial.Extensions;
using Spatial.Helpers;

namespace Spatial.Cloud.Controllers;

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
    [Path("create")]
    public Task CreateKeyAsync([Body] CreateKeyOptions options)
    {
        var key = new Key { Owner = options.User };

        key.Store();

        Smtp.GetOrCreateClient().Send(Server.Current.Configuration.SMTP.Username, key.Owner, $"Your key code is {key.Code}", key.Code);

        return Task.CompletedTask;
    }
}