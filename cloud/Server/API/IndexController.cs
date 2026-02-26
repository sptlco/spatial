// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Logistics;

namespace Spatial.Cloud.API;

/// <summary>
/// A root <see cref="Controller"/>.
/// </summary>
[Path("/")]
public class IndexController : Controller
{
    /// <summary>
    /// Get the server's Ethereum address.
    /// </summary>
    /// <returns>The server's Ethereum address.</returns>
    [GET]
    [Path("address")]
    public async Task<string> GetAddressAsync()
    {
        return Ethereum.CreateClient().Account.Address;
    }
    
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