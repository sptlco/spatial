// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Accounts;

/// <summary>
/// Configurable options for a new account.
/// </summary>
public class CreateAccountOptions : CreateResourceOptions
{
    /// <summary>
    /// The user's name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The user's email address.
    /// </summary>
    public string Email { get; set; }
}