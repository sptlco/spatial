// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Accounts;
using Spatial.Cloud.Data.Scopes;
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
    /// Create a new <see cref="Account"/>.
    /// </summary>
    /// <param name="options">Configurable options for the <see cref="Account"/>.</param>
    /// <returns>An <see cref="Account"/>.</returns>
    [POST]
    //[Authorize(Scope.Accounts.Create)]
    public async Task<Account> CreateAccountAsync([Body] CreateAccountOptions options)
    {
        if (Resource<Account>.FirstOrDefault(a => a.Email == options.Email) is Account account)
        {
            throw new UserError("The user already exists.");
        }

        account = new Account {
            Name = options.Name,
            Email = options.Email,
            Metadata = options.Metadata
        };

        return account.Store();
    }

    /// <summary>
    /// Update an account.
    /// </summary>
    /// <param name="account">The account to update.</param>
    /// <returns>The updated account.</returns>
    [PATCH]
    //[Authorize(Scope.Accounts.Update)]
    public async Task<Account> UpdateAccountAsync([Body] Account account)
    {
        return account.Save();
    }

    /// <summary>
    /// Delete an account.
    /// </summary>
    /// <param name="account">The account to delete.</param>
    [DELETE]
    [Path("{account}")]
    //[Authorize(Scope.Accounts.Delete)]
    public async Task DeleteAccountAsync(string account)
    {
        Resource<Account>.RemoveOne(a => a.Id == account);
    }
}