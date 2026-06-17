// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// Configurable options for SMTP.
/// </summary>
public class SmtpConfiguration
{
    /// <summary>
    /// The SMTP host name.
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// The outgoing SMTP port number.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// The system's display name for email.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The SMTP account username.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The SMTP account password.
    /// </summary>
    public string Password { get; set; }
}