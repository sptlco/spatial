// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Networking;

namespace Spatial.Controllers;

/// <summary>
/// A <see cref="Controller"/> for system functions.
/// </summary>
[Module]
[Endpoint("/")]
public class SystemController : Controller
{
    /// <summary>
    /// Get the system's current version.
    /// </summary>
    /// <returns>The system's current version.</returns>
    [Endpoint("version")]
    public string GetVersion()
    {
        return Configuration.Current.Version;
    }
}