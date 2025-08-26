// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Contracts.Systems.Trading;

/// <summary>
/// Configurable trading options.
/// </summary>
public class TradeConfiguration
{
    /// <summary>
    /// A list of tokens to watch.
    /// </summary>
    public List<string> Watch { get; set; } = [];
}