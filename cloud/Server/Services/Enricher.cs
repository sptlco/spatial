// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Models.Accounts;
using Spatial.Identity;
using Spatial.Persistence;

namespace Spatial.Cloud.Services;

/// <summary>
/// Adds data to the request pipeline.
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
        if (context.Items.TryGetValue("Session", out var sesh) && sesh is Session session)
        {
            context.Items["Account"] = Record<Account>.Read(session.User);
        }

        await _next(context);
    }
}