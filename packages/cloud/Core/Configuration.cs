// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Identity;
using Spatial.Logistics.Configuration;
using Spatial.Networking;
using Spatial.Persistence;

namespace Spatial;

/// <summary>
/// Configurable options for the system.
/// </summary>
public class Configuration
{
    /// <summary>
    /// The name of the <see cref="Application"/>.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The application's version.
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// The application's public endpoints.
    /// </summary>
    public string Endpoints { get; set; } = string.Empty;

    /// <summary>
    /// The base path for ASP.NET routes.
    /// </summary>
    public string? BasePath { get; set; }

    /// <summary>
    /// The system's tick rate.
    /// </summary>
    public int TickRate { get; set; } = 30;

    /// <summary>
    /// Configurable options for the system's <see cref="Resource"/> database.
    /// </summary>
    [ValidateObjectMembers]
    public DatabaseConfiguration Database { get; set; } = new DatabaseConfiguration();

    /// <summary>
    /// Configurable options for the system's <see cref="Persistence.Cache"/>.
    /// </summary>
    [ValidateObjectMembers]
    public CacheConfiguration Cache { get; set; } = new CacheConfiguration();

    /// <summary>
    /// Configurable options for <see cref="Logistics.Stripe"/>.
    /// </summary>
    [ValidateObjectMembers]
    public StripeConfiguration Stripe { get; set; } = new StripeConfiguration();

    /// <summary>
    /// Configurable options for the system's <see cref="Logistics.Ethereum"/> provider.
    /// </summary>
    [ValidateObjectMembers]
    public EthereumConfiguration Ethereum { get; set; } = new EthereumConfiguration();

    /// <summary>
    /// Configurable options for SMTP.
    /// </summary>
    [ValidateObjectMembers]
    public SmtpConfiguration SMTP { get; set; } = new SmtpConfiguration();

    /// <summary>
    /// Configurable options for JWT.
    /// </summary>
    [ValidateObjectMembers]
    public JwtConfiguration JWT { get; set; } = new JwtConfiguration();
}