// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

/// <summary>
/// An <see cref="Exception"/> that occurred during runtime.
/// </summary>
public class Fault : Exception
{
    /// <summary>
    /// Create a new <see cref="Fault"/>.
    /// </summary>
    /// <param name="error">An <see cref="Spatial.Error"/> that occurred.</param>
    public Fault(Error error) : base(error.Message)
    {
        Error = error;
    }

    /// <summary>
    /// An <see cref="Spatial.Error"/> that occurred.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Convert the <see cref="Fault"/> to a <see cref="FaultResponse"/>.
    /// </summary>
    /// <param name="request">A trace identifier.</param>
    /// <returns>A <see cref="FaultResponse"/>.</returns>
    public FaultResponse ToResponse(string request)
    {
        return new FaultResponse(this, request);
    }
}