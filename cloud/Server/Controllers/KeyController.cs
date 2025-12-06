// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Keys;
using Spatial.Cloud.Models;
using Spatial.Networking;
using Spatial.Persistence;

namespace Spatial.Cloud.Controllers;

/// <summary>
/// A <see cref="Controller"/> for keys.
/// </summary>
[Module]
[Path("keys")]
public class KeyController : Controller
{
    /// <summary>
    /// Create a new key.
    /// </summary>
    /// <param name="options">Configurable options for the key.</param>
    /// <returns>A key identifier.</returns>
    [POST]
    [Path("create")]
    public async Task<string> CreateKeyAsync([Body] CreateKeyOptions options)
    {
        // First, load the resource from the database.
        // The request has a subject identifier that we can use.

        var resource = Record<Resource>.Read(options.Subject);
        var owner = Record<Resource>.Read(resource.Owner);

        // Using the seed of the requested resource, generate a new key 
        // code and assign it to the new key.

        var key = new Resource("Key") {
            Properties = {
                [Properties.Subject] = resource.Id,
                [Properties.Code] = GenerateKeyCode(resource.Seed),
                [Properties.Expires] = (Time.Now + options.TTL).ToString()
            }
        };

        // Notify the owner of the resource that a new key has been created.
        // Use the email address and/or phone number in the owner's metadata.

        // ...

        return await Task.FromResult(key.Id);
    }

    private string GenerateKeyCode(int seed)
    {
        var random = new Random(seed);
        var characters = Constants.Digits;
        var code = string.Empty;

        for (var i = 0; i < Constants.KeyLength; i++)
        {
            code += characters[random.Next(characters.Length)];
        }

        return code;
    }
}