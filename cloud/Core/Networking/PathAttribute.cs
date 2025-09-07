// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// Defines a path on an HTTP endpoint.
/// </summary>
public class PathAttribute : Microsoft.AspNetCore.Mvc.RouteAttribute
{
    /// <summary>
    /// Create a new <see cref="PathAttribute"/>.
    /// </summary>
    /// <param name="template">The path's template.</param>
    public PathAttribute(string template) : base(template)
    {
    }
}