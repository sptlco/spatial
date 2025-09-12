// Copyright © Spatial Corporation. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace Spatial.Blockchain;

/// <summary>
/// Configurable options for the system's <see cref="Ethereum"/> provider.
/// </summary>
public class EthereumConfiguration
{
    /// <summary>
    /// An Ethereum network's RPC endpoint.
    /// </summary>
    [Required]
    public string Endpoint { get; set; }

    /// <summary>
    /// The system's private key.
    /// </summary>
    [Required]
    public string Key { get; set; }
}