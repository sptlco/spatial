// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Accounts;
using Spatial.Cloud.Data.Principals;
using Spatial.Identity;

namespace Spatial.Cloud.Data.Users;

/// <summary>
/// Contains data for a registered user.
/// </summary>
public class User
{
    /// <summary>
    /// The user's registered account.
    /// </summary>
    public Account Account { get; set; }

    /// <summary>
    /// The user's principal, used for authorization.
    /// </summary>
    public Principal Principal { get; set; }

    /// <summary>
    /// The user's current session.
    /// </summary>
    public Session Session { get; set; }
}