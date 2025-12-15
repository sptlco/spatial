// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Models.Users;

namespace Spatial.Cloud.Controllers;

/// <summary>
/// An augmented <see cref="Networking.Controller"/>.
/// </summary>
public class Controller : Networking.Controller
{
    /// <summary>
    /// The user's <see cref="Account"/>.
    /// </summary>
    protected Account Account => (HttpContext.Items["Account"] as Account)!;
}