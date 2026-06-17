// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Identity.Authorization;

/// <summary>
/// Authorizes an API endpoint.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
{
    /// <summary>
    /// Create a new <see cref="AuthorizeAttribute"/>.
    /// </summary>
    /// <param name="permissions">A list of required permissions.</param>
    public AuthorizeAttribute(params string[] permissions) : base(Constants.Policies.RBAC)
    {
        Permissions = permissions;
    }

    /// <summary>
    /// The permissions required for the endpoint.
    /// </summary>
    public string[] Permissions { get; set; }
}