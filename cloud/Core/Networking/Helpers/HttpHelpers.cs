// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking.Helpers;

/// <summary>
/// Helper methods for HTTP.
/// </summary>
public static class Http
{
    private static HttpClient? _client;

    /// <summary>
    /// Create a new <see cref="HttpClient"/>.
    /// </summary>
    /// <returns>An <see cref="HttpClient"/>.</returns>
    public static HttpClient GetOrCreateClient()
    {
        return _client ??= new HttpClient();
    }
}