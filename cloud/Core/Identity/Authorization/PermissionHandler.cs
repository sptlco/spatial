// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Spatial.Identity.Authorization;

/// <summary>
/// Handles a <see cref="PermissionRequirement"/>.
/// </summary>
public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    /// <summary>
    /// Handle the <see cref="PermissionRequirement"/>.
    /// </summary>
    /// <param name="context">The current <see cref="AuthorizationHandlerContext"/>.</param>
    /// <param name="requirement">The <see cref="PermissionRequirement"/> to handle.</param>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            if (context.Resource is HttpContext http)
            {
                var endpoint = http.GetEndpoint();

                if (endpoint?.Metadata.GetMetadata<AuthorizeAttribute>()?.Permissions is string[] permissions)
                {
                    if (permissions.Length <= 0 || permissions.Any(permission => context.User.HasClaim(Claims.Permission, permission)))
                    {
                        context.Succeed(requirement);
                    }
                }
            }
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}