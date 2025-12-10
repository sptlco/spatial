// Copyright © Spatial Corporation. All rights reserved.

using System.Net;
using System.Net.Mail;

namespace Spatial.Helpers;

/// <summary>
/// Helper methods for SMTP.
/// </summary>
public static class Smtp
{
    private static SmtpClient? _client;

    /// <summary>
    /// Create a new <see cref="SmtpClient"/>.
    /// </summary>
    /// <returns>An <see cref="SmtpClient"/>.</returns>
    public static SmtpClient GetOrCreateClient()
    {
        if (_client is null)
        {
            var config = Application.Current.Configuration;

            _client = new SmtpClient(config.SMTP.Host) {
                Port = config.SMTP.Port,
                Credentials = new NetworkCredential(config.SMTP.Username, config.SMTP.Password),
                EnableSsl = true
            };
        }

        return _client;
    }
}