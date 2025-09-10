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
        if (_client is null)
        {
            var config = Application.Current.Configuration;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", $"{config.Name}/{config.Version}");
        }

        return _client;
    }
}