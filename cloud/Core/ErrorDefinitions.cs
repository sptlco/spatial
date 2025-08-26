// Copyright © Spatial Corporation. All rights reserved.

using System.Net;

namespace Spatial;

/// <summary>
/// An <see cref="Error"/> on behalf of the system.
/// </summary>
public class SystemError : Error
{
    /// <summary>
    /// Create a new <see cref="SystemError"/>.
    /// </summary>
    /// <param name="message">A message describing the <see cref="Error"/>.</param>
    /// <param name="status">The status of a request after the <see cref="Error"/> occurred.</param>
    public SystemError(string message, int status = (int) HttpStatusCode.InternalServerError) : base(message, status) { }
}

/// <summary>
/// A <see cref="SystemError"/> indicating that an object reference is not set 
/// to an instance of an object.
/// </summary>
public class NullReference : SystemError
{
    /// <summary>
    /// Create a new <see cref="NullReference"/>.
    /// </summary>
    public NullReference() : base("The object reference is not set to an instance of an object.") { }
}

/// <summary>
/// A <see cref="SystemError"/> that indicates a method was called with invalid parameters.
/// </summary>
public class InvalidParameters : SystemError
{
    /// <summary>
    /// Create a new <see cref="InvalidParameters"/>.
    /// </summary>
    public InvalidParameters() : base("Invalid values provided for one or more parameters.") { }

    /// <summary>
    /// Create a new <see cref="InvalidParameters"/>.
    /// </summary>
    /// <param name="parameters">The names of the invalid parameters.</param>
    public InvalidParameters(params string[] parameters) : base($"Invalid values provided for parameters '{string.Join(", ", parameters)}'.") { }
}

/// <summary>
/// An <see cref="Error"/> on behalf of the user.
/// </summary>
public class UserError : Error
{
    /// <summary>
    /// Create a new <see cref="UserError"/>.
    /// </summary>
    /// <param name="message">A message describing the <see cref="Error"/>.</param>
    /// <param name="status">The status of a request after the <see cref="Error"/> occurred.</param>
    public UserError(string message, int status = (int) HttpStatusCode.BadRequest) : base(message, status) { }
}

/// <summary>
/// A <see cref="UserError"/> indicating a resource's nonexistance.
/// </summary>
public class NotFound : UserError
{
    /// <summary>
    /// Create a new <see cref="NotFound"/>.
    /// </summary>
    public NotFound() : base("The requested resource does not exist.", 404) { }
}