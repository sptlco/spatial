// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Controllers;

/// <summary>
/// A root <see cref="Controller"/>.
/// </summary>
[Path("/")]
public class IndexController : Controller
{
    /// <summary>
    /// Get the server's name.
    /// </summary>
    /// <returns>The server's name.</returns>
    [GET]
    [Path("name")]
    public async Task<string> GetNameAsync()
    {
        return Configuration.Current.Name;
    }

    /// <summary>
    /// Get the server's version.
    /// </summary>
    /// <returns>The server's version.</returns>
    [GET]
    [Path("version")]
    public async Task<string> GetVersionAsync()
    {
        return Configuration.Current.Version;
    }
}