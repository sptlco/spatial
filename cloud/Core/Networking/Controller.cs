// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Http;
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
    /// Indicates that a parameter is read from a query parameter.
    /// </summary>
    public class QueryAttribute : FromQueryAttribute
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

    /// <summary>
    /// The result of an operation.
    /// </summary>
    public abstract class Result : IResult
    {
        /// <summary>
        /// Execute the result pipeline.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        public async Task ExecuteAsync(HttpContext httpContext)
        {
            var value = GetValue();
            var status = GetStatusCode();

            if (status == StatusCodes.Status204NoContent)
            {
                httpContext.Response.StatusCode = status;
                return;
            }

            await TypedResults.Json(value, statusCode: status).ExecuteAsync(httpContext);
        }

        /// <summary>
        /// Signal successful completion of an operation.
        /// </summary>
        /// <param name="value">The result's value.</param>
        /// <returns>The operation's result.</returns>
        public static Result OK(object? value = null) => new JsonResult(value, StatusCodes.Status200OK);

        /// <summary>
        /// Signal successful creation of a resource.
        /// </summary>
        /// <param name="value">The result's value.</param>
        /// <returns>The operation's result.</returns>
        public static Result Created(object? value) => new JsonResult(value, StatusCodes.Status201Created);

        /// <summary>
        /// Signal successful completion of an operation.
        /// </summary>
        /// <returns>An operation result.</returns>
        public static Result NoContent() => new StatusResult(StatusCodes.Status204NoContent);

        /// <summary>
        /// Create a new <see cref="Result"/> from a value.
        /// </summary>
        /// <param name="value">The result's value.</param>
        /// <returns>The operation's result.</returns>
        public static Result FromValue(object? value) => OK(value);

        /// <summary>
        /// Trigger the fault pipeline.
        /// </summary>
        /// <param name="error"></param>
        public static implicit operator Result(Error error) => throw error;

        internal abstract object? GetValue();
        internal virtual int GetStatusCode() => StatusCodes.Status200OK;

        private sealed class JsonResult : Result
        {
            private readonly object? _value;
            private readonly int _status;

            /// <summary>
            /// Create a new <see cref="JsonResult"/>.
            /// </summary>
            /// <param name="value">The result's value.</param>
            /// <param name="status">The result's status code.</param>
            public JsonResult(object? value, int status)
            {
                _value = value;
                _status = status;
            }

            internal override object? GetValue() => _value;
            internal override int GetStatusCode() => _status;
        }

        private sealed class StatusResult : Result
        {
            private readonly int _status;

            /// <summary>
            /// Create a new <see cref="StatusResult"/>.
            /// </summary>
            /// <param name="status">A status code.</param>
            public StatusResult(int status)
            {
                _status = status;
            }

            internal override object? GetValue() => null;
            internal override int GetStatusCode() => _status;
        }
    }

    /// <summary>
    /// A typed <see cref="Result"/>.
    /// </summary>
    /// <typeparam name="T">The result's data type.</typeparam>
    public sealed class Result<T> : Result
    {
        private readonly T? _value;
        private readonly int _status;

        private Result(T? value, int status)
        {
            _value = value;
            _status = status;
        }

        /// <summary>
        /// Signal successful completion of an operation.
        /// </summary>
        /// <param name="value">The result's value.</param>
        /// <returns>The operation's result.</returns>
        public static Result<T> OK(T value) => new(value, StatusCodes.Status200OK);

        /// <summary>
        /// Signal successful creation of a resource.
        /// </summary>
        /// <param name="value">The result's value.</param>
        /// <returns>The operation's result.</returns>
        public static Result<T> Created(T value) => new(value, StatusCodes.Status201Created);

        /// <summary>
        /// Create a new <see cref="Result"/> from a value.
        /// </summary>
        /// <param name="value">The result's value.</param>
        /// <returns>The operation's result.</returns>
        public static Result<T> FromValue(T value) => OK(value);

        /// <summary>
        /// Create a new <see cref="Result"/>.
        /// </summary>
        /// <param name="value">The result's value.</param>
        public static implicit operator Result<T>(T value) => new(value, StatusCodes.Status200OK);

        /// <summary>
        /// Trigger the fault pipeline.
        /// </summary>
        /// <param name="error"></param>
        public static implicit operator Result<T>(Error error) => throw error;

        internal override object? GetValue() => _value;
        internal override int GetStatusCode() => _status;
    }

    /// <summary>
    /// Helper methods for fault signaling
    /// </summary>
    public static class Fail
    {
        /// <summary>
        /// Abort the current operation by throwing an error.
        /// </summary>
        /// <param name="error">An <see cref="Error"/> to throw.</param>
        public static void With(Error error)
        {
            throw error;
        }
    }

    // ...
}