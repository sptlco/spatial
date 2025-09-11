// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Blockchain.Helpers;

namespace Spatial.Cloud.Systems.Banking.Engines;

/// <summary>
/// A default <see cref="IRecommender"/> that generates a <see cref="Recommendation"/> 
/// using a local investment algorithm.
/// </summary>
internal class DefaultRecommender : IRecommender
{
    /// <summary>
    /// Generate a list of trade recommendations.
    /// </summary>
    /// <param name="coins">Market data for each coin.</param>
    /// <returns>A list of trade recommendations./returns>
    public async Task<List<Recommendation>> NextAsync(List<CoinGecko.Coin> coins)
    {
        return await Task.FromResult<List<Recommendation>>([]);
    }
}