// Copyright © Spatial Corporation. All rights reserved.

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
}