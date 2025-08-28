// Copyright © Spatial Corporation. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace Spatial.Blockchain;

/// <summary>
/// Configurable options for the system's <see cref="Ethereum"/> provider.
/// </summary>
public class EthereumConfiguration
{
    /// <summary>
    /// The system's Infura URL.
    /// </summary>
    [Required]
    public string Url { get; set; }

    /// <summary>
    /// The system's address.
    /// </summary>
    [Required]
    public string Address { get; set; }
}