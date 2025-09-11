// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Blockchain.Helpers;

namespace Spatial.Cloud.Systems.Banking;

/// <summary>
/// A trade <see cref="Recommendation"/> generator.
/// </summary>
internal interface IRecommender
{
    // <summary>
    /// Generate a list of trade recommendations.
    /// </summary>
    /// <param name="watchlist">A list of coins of interest.</param>
    /// <returns>A list of recommendations.</returns>
    Task<List<Recommendation>> NextAsync(List<CoinGecko.Coin> coins);
}