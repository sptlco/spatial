// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Blockchain;
using Spatial.Intelligence;
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
    [Required]
    public string[] Endpoints { get; set; }

    /// <summary>
    /// The system's tick rate.
    /// </summary>
    public int TickRate { get; set; } = 30;

    /// <summary>
    /// Configurable options for the system's <see cref="Document"/> database.
    /// </summary>
    [ValidateObjectMembers]
    public DatabaseConfiguration Database { get; set; } = new DatabaseConfiguration();

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