// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// A means of communication using the hyper-text transfer protocol (HTTP).
/// </summary>
public class Http
{
    private HttpClient? _client;

    /// <summary>
    /// Create a new <see cref="HttpClient"/>.
    /// </summary>
    /// <returns>An <see cref="HttpClient"/>.</returns>
    public HttpClient CreateClient()
    {
        return _client ??= new HttpClient();
    }
}
