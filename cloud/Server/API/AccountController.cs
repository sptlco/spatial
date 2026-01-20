// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Accounts;
using Spatial.Extensions;
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

    /// <summary>
    /// Create a new <see cref="Account"/>.
    /// </summary>
    /// <param name="options">Configurable options for the <see cref="Account"/>.</param>
    /// <returns>An <see cref="Account"/>.</returns>
    [POST]
    public async Task<Account> CreateAccountAsync([Body] CreateAccountOptions options)
    {
        var account = new Account {
            Name = options.Name,
            Email = options.Email
        };

        account.Store();

        return account;
    }

    /// <summary>
    /// Update the current account.
    /// </summary>
    /// <param name="account">An account update.</param>
    /// <returns>The updated account.</returns>
    [PATCH]
    [Path("me")]
    [Authorize]
    public async Task<Account> UpdateAccountAsync([Body] Account account)
    {
        account.Save();

        return account;
    }

    /// <summary>
    /// Delete an account.
    /// </summary>
    /// <param name="id">The account to delete.</param>
    [DELETE]
    [Path("{id}")]
    [Authorize]
    public async Task DeleteAccountAsync(string id)
    {
        Record<Account>.RemoveOne(account => account.Id == id);
    }
}