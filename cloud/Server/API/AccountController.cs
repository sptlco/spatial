// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Accounts;
using Spatial.Identity.Authorization;
using Spatial.Persistence;

namespace Spatial.Cloud.API;

/// <summary>
/// A controller for account functions.
/// </summary>
[Path("accounts")]
public class AccountController : Controller
{
    /// <summary>
    /// Get the current account.
    /// </summary>
    /// <returns>The current account.</returns>
    [GET]
    [Path("me")]
    [Authorize]
    public async Task<Account> GetAccountAsync()
    {
        return _account;
    }

    /// <summary>
    /// Get an account.
    /// </summary>
    /// <param name="id">An account identifier.</param>
    /// <returns>The specified <see cref="Account"/>.</returns>
    [GET]
    [Path("{id}")]
    [Authorize(Scope.Accounts.Read)]
    public async Task<Account> GetAccountAsync(string id)
    {
        return await Task.FromResult(Record<Account>.Read(id));
    }
}