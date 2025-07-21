// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Blockchain;

/// <summary>
/// Configurable options for the system's <see cref="Ethereum"/> provider.
/// </summary>
public class EthereumConfiguration
{
    /// <summary>
    /// The system's Infura URL.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// The system's private key.
    /// </summary>
    public string PrivateKey { get; set; }
}