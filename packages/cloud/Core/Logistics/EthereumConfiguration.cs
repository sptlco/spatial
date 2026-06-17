// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Logistics;

/// <summary>
/// Configurable options for the system's <see cref="Ethereum"/> provider.
/// </summary>
public class EthereumConfiguration
{
    /// <summary>
    /// Configurable options for the <see cref="Allocator"/>.
    /// </summary>
    public AllocatorConfiguration Allocator { get; set; } = new AllocatorConfiguration();
    
    /// <summary>
    /// An Ethereum network's RPC endpoint.
    /// </summary>
    public string Endpoint { get; set; }

    /// <summary>
    /// The system's private key.
    /// </summary>
    public string Key { get; set; }
}