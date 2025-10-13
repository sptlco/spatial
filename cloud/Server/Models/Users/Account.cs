// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Cloud.Models.Users;

/// <summary>
/// A unique identity known to the <see cref="Server"/>.
/// </summary>
[Collection("accounts")]
public class Account : Record
{
    // ...
}