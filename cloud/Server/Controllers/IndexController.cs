// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Networking;

namespace Spatial.Cloud.Controllers;

/// <summary>
/// A <see cref="Controller"/> for <see cref="Server"/>.
/// </summary>
[Module]
[Path("/")]
public class ServerController : Controller
{
    /// <summary>
    /// Get the server's name.
    /// </summary>
    /// <returns>The server's name.</returns>
    [GET]
    [Path("name")]
    public string GetName()
    {
        return Configuration.Current.Name;
    }

    /// <summary>
    /// Get the server's version.
    /// </summary>
    /// <returns>The server's version.</returns>
    [GET]
    [Path("version")]
    public string GetVersion()
    {
        return Configuration.Current.Version;
    }
}