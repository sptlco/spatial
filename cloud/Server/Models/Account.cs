// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Cloud.Models;

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