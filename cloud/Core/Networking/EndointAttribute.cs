// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// Establishes an endpoint for HTTP requests.
/// </summary>
public class EndpointAttribute : Microsoft.AspNetCore.Mvc.RouteAttribute
{
    /// <summary>
    /// Create a new <see cref="EndpointAttribute"/>.
    /// </summary>
    /// <param name="template">The endpoint's path template.</param>
    public EndpointAttribute(string template) : base(template)
    {
    }
}