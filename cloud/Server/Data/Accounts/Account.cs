// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Cloud.Data.Accounts;

/// <summary>
/// ...
/// </summary>
[Collection("accounts")]
public class Account : Record
{
    /// <summary>
    /// The user's email address.
    /// </summary>
    public string Email { get; set; }
}