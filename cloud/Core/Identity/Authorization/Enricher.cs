// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Http;
using Spatial.Persistence;
using System.Security.Claims;

namespace Spatial.Identity.Authorization;

/// <summary>
/// Loads the user's current roles and permissions.
/// </summary>
public class Enricher
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Create a new <see cref="Enricher"/>.
    /// </summary>
    /// <param name="next">The next request handler.</param>
    public Enricher(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invoke the <see cref="Enricher"/>.
    /// </summary>
    /// <param name="context">The current <see cref="HttpContext"/>.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Items.TryGetValue(Variables.Session, out var sesh) && sesh is Session session)
        {
            var roles = Resource<Assignment>
                .List(a => a.User == session.User)
                .Select(a => Resource<Role>.Read(a.Role));

            var permissions = roles
                .SelectMany(r => Resource<Permission>.List(p => p.Role == r.Id))
                .Select(p => p.Scope)
                .Distinct();

            if (context.User.Identity is ClaimsIdentity identity)
            {
                identity.AddClaims(roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r.Name)));
                identity.AddClaims(permissions.Select(p => new Claim(Claims.Permission, p)));
            }
        }

        await _next(context);
    }
}