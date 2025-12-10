// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Spatial.Identity;

/// <summary>
/// A secure token used for authentication purposes.
/// </summary>
public class Token
{
    /// <summary>
    /// Create a new security token.
    /// </summary>
    /// <param name="subject">The token's subject.</param>
    /// <param name="email">The token bearer's email address.</param>
    /// <returns>The security token that was created.</returns>
    public static string Create(string subject, string email)
    {
        var config = Application.Current.Configuration;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.JWT.Secret));

        var claims = new List<Claim> {
          new(JwtRegisteredClaimNames.Sub, subject),
          new(JwtRegisteredClaimNames.Email, email)  
        };

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