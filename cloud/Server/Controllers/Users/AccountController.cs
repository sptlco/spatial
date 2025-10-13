// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Users;
using Spatial.Networking;

namespace Spatial.Cloud.Controllers.Users;

/// <summary>
/// A controller for <see cref="Account"/>.
/// </summary>
[Module]
[Path("accounts")]
public class AccountController : Controller
{
    /// <summary>
    /// Create a new <see cref="Account"/>.
    /// </summary>
    /// <param name="credentials">The account's <see cref="Credentials"/>.</param>
    /// <returns>An <see cref="Account"/>.</returns>
    [POST]
    [Path("/")]
    public async Task<Account> CreateAccountAsync(Credentials credentials)
    {
        // ...

        return await Task.FromResult(new Account());
    }
}