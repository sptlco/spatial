// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

/// <summary>
/// The system's response to a <see cref="Fault"/>.
/// </summary>
public class FaultResponse
{
    /// <summary>
    /// Create a new <see cref="FaultResponse"/>.
    /// </summary>
    /// <param name="fault">A <see cref="Fault"/> that occurred.</param>
    /// <param name="request">A trace identifier.</param>
    public FaultResponse(Fault fault, string request)
    {
        Time = fault.Error.Time;
        Request = request;
        Status = fault.Error.Status;
        Code = fault.Error.Code;
        Message = fault.Error.Message;
    }

    /// <summary>
    /// The <see cref="Spatial.Time"/> the <see cref="Error"/> occurred.
    /// </summary>
    public double Time { get; }

    /// <summary>
    /// The request's trace identifier.
    /// </summary>
    public string Request { get; }

    /// <summary>
    /// The status of a request after the <see cref="Error"/> occurred.
    /// </summary>
    public int Status { get; }

    /// <summary>
    /// A classifying <see cref="Error"/> code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// A message describing what went wrong.
    /// </summary>
    public string Message { get; }
}