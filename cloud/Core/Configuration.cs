// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Billing;
using Spatial.Blockchain;
using Spatial.Caching;
using Spatial.Intelligence.ThirdParty;
using Spatial.Persistence;
using System.ComponentModel.DataAnnotations;

namespace Spatial;

/// <summary>
/// Configurable options for the system.
/// </summary>
public class Configuration
{
    /// <summary>
    /// Get the current <see cref="Configuration"/>.
    /// </summary>
    public static Configuration Current => Application.Current.Configuration;

    /// <summary>
    /// The name of the <see cref="Application"/>.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// The application's version.
    /// </summary>
    [Required]
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
    /// Configurable options for the system's <see cref="Record"/> database.
    /// </summary>
    [ValidateObjectMembers]
    public DatabaseConfiguration Database { get; set; } = new DatabaseConfiguration();

    /// <summary>
    /// Configurable options for the system's <see cref="Caching.Cache"/>.
    /// </summary>
    [ValidateObjectMembers]
    public CacheConfiguration Cache { get; set; } = new CacheConfiguration();

    /// <summary>
    /// Configurable options for <see cref="Billing.Stripe"/>.
    /// </summary>
    [ValidateObjectMembers]
    public StripeConfiguration Stripe { get; set; } = new StripeConfiguration();

    /// <summary>
    /// Configurable options for the system's <see cref="Blockchain.Ethereum"/> provider.
    /// </summary>
    [ValidateObjectMembers]
    public EthereumConfiguration Ethereum { get; set; } = new EthereumConfiguration();

    /// <summary>
    /// Configurable options for OpenAI.
    /// </summary>
    [ValidateObjectMembers]
    public OpenAIConfiguration OpenAI { get; set; } = new OpenAIConfiguration();
}