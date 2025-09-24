// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Networking;

namespace Spatial.Cloud.Controllers;

/// <summary>
/// A control plane for the <see cref="Server"/>.
/// </summary>
[Module]
[Path("/")]
public class ServerController : Controller
{
    /// <summary>
    /// Get the server's current name.
    /// </summary>
    /// <returns>The server's current name.</returns>
    [Path("name")]
    public string GetName()
    {
        return Configuration.Current.Name;
    }

    /// <summary>
    /// Get the server's current version.
    /// </summary>
    /// <returns>The server's current version.</returns>
    [Path("version")]
    public string GetVersion()
    {
        return Configuration.Current.Version;
    }
}