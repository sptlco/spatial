// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Spatial.Helpers;

/// <summary>
/// Helper methods for JSON web tokens (JWT).
/// </summary>
public static class JWT
{
    /// <summary>
    /// Create a new security token.
    /// </summary>
    /// <param name="claims">A list of security claims.</param>
    /// <returns>The security token that was created.</returns>
    public static string Create(List<Claim> claims)
    {
        var config = Application.Current.Configuration;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.JWT.Secret));

        var descriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(config.JWT.TTL),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
            Issuer = config.JWT.Issuer,
            Audience = config.JWT.Audience
        };

        return new JsonWebTokenHandler().CreateToken(descriptor);
    }
}