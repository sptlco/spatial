// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Transmissions;
using Spatial.Persistence;

namespace Spatial.Cloud.API;

/// <summary>
/// A <see cref="Controller"/> for <see cref="Transmission"/> functions.
/// </summary>
[Path("Transmissions")]
public class TransmissionController : Controller
{
    /// <summary>
    /// Stream a <see cref="Transmission"/>.
    /// </summary>
    /// <param name="transmission">The <see cref="Transmission"/> to stream.</param>
    /// <param name="password">The transmission's password.</param>
    /// <returns>The requested <see cref="Transmission"/>.</returns>
    [POST]
    [Path("{transmission}")]
    public async Task StreamTransmissionAsync(string transmission, [Body] string password)
    {
        var resource = await Resource<Transmission>.ReadAsync(transmission);

        if (!BCrypt.Net.BCrypt.Verify(password, resource.Passphrase))
        {
            throw new Unauthorized();
        }

        var path = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "transmissions", resource.Path);
        var file = new FileInfo(path);

        if (!file.Exists)
        {
            throw new NotFound();
        }

        Response.StatusCode = 200;  
        Response.ContentType = "video/mp4";
        Response.ContentLength = file.Length;
        Response.Headers.AcceptRanges = "bytes";

        using var stream = file.OpenRead();

        await stream.CopyToAsync(Response.Body);
    }
}