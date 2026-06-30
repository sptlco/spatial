// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Controllers;

/// <summary>
/// A <see cref="Controller"/> for the <see cref="Application"/>.
/// </summary>
[Path("/")]
public class ApplicationController : Controller
{
    /// <summary>
    /// Get the application's name.
    /// </summary>
    /// <returns>The application's name.</returns>
    [Path("name")]
    public Task<string> GetNameAsync()
    {
        return Task.FromResult(Server.Current.Configuration.Name);
    }

    /// <summary>
    /// Get the application's version.
    /// </summary>
    /// <returns>The application's semantic version.</returns>
    [Path("version")]
    public Task<string> GetVersionAsync()
    {
        return Task.FromResult(Server.Current.Configuration.Version);
    }
}