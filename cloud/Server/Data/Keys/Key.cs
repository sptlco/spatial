// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Accounts;
using Spatial.Persistence;

namespace Spatial.Cloud.Data.Keys;

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
    public double Expires { get; set; } = Time.Now + Time.FromMinutes(10);

    private static string GenerateCode()
    {
        var code = string.Empty;

        for (var i = 0; i < Constants.KeyLength; i++)
        {
            code += Strong.Int32(10).ToString();
        }

        return code;
    }
}