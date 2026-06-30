// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Http;
using System.Text;

namespace Spatial;

/// <summary>
/// Tests for a <see cref="FaultIndicator"/>.
/// </summary>
public class FaultIndicatorTests
{
    /// <summary>
    /// Verifies that <see cref="FaultIndicator.TryHandleAsync"/> returns false and leaves
    /// the response untouched when the exception is not a <see cref="Fault"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public async Task TestTryHandleAsync_ReturnsFalse_WhenExceptionIsNotFault()
    {
        var indicator = new FaultIndicator();
        var context = CreateHttpContext();
        var exception = new InvalidOperationException("Not a fault.");

        var handled = await indicator.TryHandleAsync(context, exception, CancellationToken.None);

        Assert.False(handled);
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        Assert.Equal(0, context.Response.Body.Length);
    }

    /// <summary>
    /// Verifies that <see cref="FaultIndicator.TryHandleAsync"/> returns true when the
    /// exception is a <see cref="Fault"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public async Task TestTryHandleAsync_ReturnsTrue_WhenExceptionIsFault()
    {
        var indicator = new FaultIndicator();
        var context = CreateHttpContext();
        var fault = CreateFault("test_error", StatusCodes.Status400BadRequest);

        var handled = await indicator.TryHandleAsync(context, fault, CancellationToken.None);

        Assert.True(handled);
    }

    /// <summary>
    /// Verifies that <see cref="FaultIndicator.TryHandleAsync"/> sets the response status
    /// code to the <see cref="Fault"/>'s error status.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public async Task TestTryHandleAsync_SetsResponseStatusCode_WhenExceptionIsFault()
    {
        var indicator = new FaultIndicator();
        var context = CreateHttpContext();
        var fault = CreateFault("test_error", StatusCodes.Status409Conflict);

        await indicator.TryHandleAsync(context, fault, CancellationToken.None);

        Assert.Equal(StatusCodes.Status409Conflict, context.Response.StatusCode);
    }

    /// <summary>
    /// Verifies that <see cref="FaultIndicator.TryHandleAsync"/> writes a JSON response body
    /// containing the request's trace identifier and the <see cref="Fault"/>'s error code.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public async Task TestTryHandleAsync_WritesJsonResponseBody_WhenExceptionIsFault()
    {
        var indicator = new FaultIndicator();
        var context = CreateHttpContext();
        context.TraceIdentifier = "trace-123";
        var fault = CreateFault("test_error", StatusCodes.Status400BadRequest);

        await indicator.TryHandleAsync(context, fault, CancellationToken.None);

        var body = await ReadResponseBodyAsync(context);

        Assert.StartsWith("application/json", context.Response.ContentType);
        Assert.Contains("trace-123", body);
        Assert.Contains("test_error", body);
    }

    /// <summary>
    /// Verifies that <see cref="FaultIndicator.TryHandleAsync"/> does not write to the
    /// response body when the exception is not a <see cref="Fault"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public async Task TestTryHandleAsync_DoesNotWriteResponse_WhenExceptionIsNotFault()
    {
        var indicator = new FaultIndicator();
        var context = CreateHttpContext();
        var exception = new InvalidOperationException("Not a fault.");

        await indicator.TryHandleAsync(context, exception, CancellationToken.None);

        var body = await ReadResponseBodyAsync(context);

        Assert.Empty(body);
    }

    private static DefaultHttpContext CreateHttpContext()
    {
        return new DefaultHttpContext {
            Response = { Body = new MemoryStream() },
        };
    }

    private static async Task<string> ReadResponseBodyAsync(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }

    private static Fault CreateFault(string code, int status)
    {
        return new SystemError(code, status).ToFault();
    }
}