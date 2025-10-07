// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Mvc;

namespace Spatial.Networking;

/// <summary>
/// A device through which a client communicates with the <see cref="Network"/>.
/// </summary>
public abstract class Controller : Microsoft.AspNetCore.Mvc.Controller
{
    /// <summary>
    /// The active <see cref="Networking.Connection"/>.
    /// </summary>
    public Connection Connection { get; internal set; }

    /// <summary>
    /// A <see cref="Networking.Message"/> sent to the <see cref="Network"/>.
    /// </summary>
    public Message Message { get; internal set; }

    /// <summary>
    /// Identifies a route that supports HTTP POST.
    /// </summary>
    public class POSTAttribute : HttpPostAttribute { }

    /// <summary>
    /// Identifies a route that supports HTTP GET.
    /// </summary>
    public class GETAttribute : HttpGetAttribute { }

    /// <summary>
    /// Identifies a route that supports HTTP PATCH.
    /// </summary>
    public class PATCHAttribute : HttpPatchAttribute { }

    /// <summary>
    /// Identifies a route that supports HTTP DELETE.
    /// </summary>
    public class DELETEAttribute : HttpDeleteAttribute { }
}