// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Keys;
using Spatial.Cloud.Models;
using Spatial.Extensions;
using Spatial.Networking;

namespace Spatial.Cloud.Controllers;

/// <summary>
/// A <see cref="Controller"/> for keys.
/// </summary>
[Module]
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
    public async Task CreateKeyAsync([Body] CreateKeyOptions options)
    {
        var key = new Key { Owner = options.UID };

        key.Save();

        // ...

        await Task.CompletedTask;
    }
}