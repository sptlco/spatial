// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Models.Users;
using Spatial.Persistence;

namespace Spatial.Cloud.Models;

/// <summary>
/// An alphanumeric code used for authorization.
/// </summary>
[Collection("keys")]
public class Key : Record
{
    /// <summary>
    /// The <see cref="Account"/> that owns the <see cref="Key"/>.
    /// </summary>
    public string Owner { get; set; }

    /// <summary>
    /// The key's alphanumeric code.
    /// </summary>
    public string Code { get; set; } = GenerateCode();

    /// <summary>
    /// The time the <see cref="Key"/> expires.
    /// </summary>
    public double Expires { get; set; } = Time.Now + Time.FromSeconds(30);

    private static string GenerateCode()
    {
        var random = new Random();
        var characters = Constants.Alphanumerics;
        var code = string.Empty;

        for (var i = 0; i < Constants.KeyLength; i++)
        {
            code += characters[random.Next(characters.Length)];
        }

        return code;
    }
}