// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Spatial;

/// <summary>
/// A global <see cref="IExceptionHandler"/> for HTTP requests.
/// </summary>
public class FaultHandler : IExceptionHandler
{
    /// <summary>
    /// Handle an <see cref="Exception"/>.
    /// </summary>
    /// <param name="context">The current <see cref="HttpContext"/>.</param>
    /// <param name="exception">The <see cref="Exception"/> that occurred.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> for the request.</param>
    /// <returns>False to continue with the default behavior; true if the exception was handled.</returns>
    public ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not Fault fault)
        {
            return ValueTask.FromResult(false);
        }

        ERROR(fault, "A fault occurred.");

        // ...

        return ValueTask.FromResult(true);
    }
}