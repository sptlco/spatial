// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Blockchain.Helpers;

namespace Spatial.Cloud.Models.Banking;

/// <summary>
/// Market data analysis of a <see cref="CoinGecko.Coin"/>.
/// </summary>
internal class Analysis
{
    /// <summary>
    /// Create a new <see cref="Analysis"/>.
    /// </summary>
    /// <param name="coin">The <see cref="CoinGecko.Coin"/> that was analyzed.</param>
    public Analysis(CoinGecko.Coin coin)
    {
        Coin = coin;
    }

    /// <summary>
    /// The <see cref="CoinGecko.Coin"/> that was analyzed.
    /// </summary>
    public CoinGecko.Coin Coin { get; }

    /// <summary>
    /// The model that analyzed the <see cref="CoinGecko.Coin"/>.
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// A trading score recommended by the system.
    /// </summary>
    public double Score { get; set; }

    /// <summary>
    /// The size of the recommended trade.
    /// </summary>
    public decimal Size { get; set; }
}