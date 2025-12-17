// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Mvc;
using Spatial.Identity;

namespace Spatial.Networking;

/// <summary>
/// A device through which a client communicates with the <see cref="Network"/>.
/// </summary>
[ApiController]
public abstract class Controller : Microsoft.AspNetCore.Mvc.Controller
{
    /// <summary>
    /// The current <see cref="_session"/>.
    /// </summary>
    protected Session _session => HttpContext.Items[Variables.Session] as Session ?? throw new NullReference();

    /// <summary>
    /// The active <see cref="Networking.Connection"/>.
    /// </summary>
    public Connection Connection { get; internal set; }

    /// <summary>
    /// A <see cref="Networking.Message"/> sent to the <see cref="Network"/>.
    /// </summary>
    public Message Message { get; internal set; }

    /// <summary>
    /// Identifies a route that supports HTTP POST.
    /// </summary>
    public class POSTAttribute : HttpPostAttribute { }

    /// <summary>
    /// Identifies a route that supports HTTP GET.
    /// </summary>
    public class GETAttribute : HttpGetAttribute { }

    /// <summary>
    /// Identifies a route that supports HTTP PATCH.
    /// </summary>
    public class PATCHAttribute : HttpPatchAttribute { }

    /// <summary>
    /// Identifies a route that supports HTTP DELETE.
    /// </summary>
    public class DELETEAttribute : HttpDeleteAttribute { }

    /// <summary>
    /// Defines a path on an HTTP endpoint.
    /// </summary>
    public class PathAttribute : RouteAttribute
    {
        /// <summary>
        /// Create a new <see cref="PathAttribute"/>.
        /// </summary>
        /// <param name="template">The path's template.</param>
        public PathAttribute(string template) : base(template)
        {
        }
    }

    /// <summary>
    /// Indicates that a <see cref="Controller"/> endpoint parameter is read from a JSON message body.
    /// </summary>
    public class BodyAttribute : FromBodyAttribute
    {
    }

    /// <summary>
    /// A command issued over the private network.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class OperationAttribute : Attribute
    {
        /// <summary>
        /// Create a new <see cref="OperationAttribute"/>.
        /// </summary>
        /// <param name="code">The operation's code number.</param>
        public OperationAttribute(ushort code)
        {
            Code = code;
        }

        /// <summary>
        /// The operation's code number.
        /// </summary>
        public ushort Code { get; }
    }
}