// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Accounts;

namespace Spatial.Cloud.API;

/// <summary>
/// An augmented <see cref="Networking.Controller"/>.
/// </summary>
public class Controller : Networking.Controller
{
    /// <summary>
    /// The user's <see cref="_account"/>.
    /// </summary>
    protected Account _account => HttpContext.Items[Variables.Account] as Account ?? throw new NullReference();
}