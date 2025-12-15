// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.IdentityModel.JsonWebTokens;
using Spatial.Cloud.Models.Users;
using Spatial.Identity;
using Spatial.Persistence;

namespace Spatial.Cloud.Middleware;

/// <summary>
/// Adds account information to the request pipeline.
/// </summary>
public class AccountResolver
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Create a new <see cref="AccountResolver"/>.
    /// </summary>
    /// <param name="next">The next request handler.</param>
    public AccountResolver(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invoke the <see cref="AccountResolver"/>.
    /// </summary>
    /// <param name="context">The current <see cref="HttpContext"/>.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var sid = context.User.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sid)?.Value;

            if (!string.IsNullOrEmpty(sid))
            {
                var session = Record<Session>.Read(sid);

                context.Items["Session"] = session;
                context.Items["Account"] = Record<Account>.Read(session.User);
            }
        }

        await _next(context);
    }
}