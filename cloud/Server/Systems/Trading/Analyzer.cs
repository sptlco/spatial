// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Blockchain.Helpers;
using Spatial.Cloud.Systems.Trading.Recommendations;

namespace Spatial.Cloud.Systems.Trading;

/// <summary>
/// A market data analyzer for ERC20 tokens.
/// </summary>
internal static class Analyzer
{
    /// <summary>
    /// Analyze a list of coins for trade recommendations.
    /// </summary>
    /// <param name="coins">A list of coins.</param>
    /// <returns>A list of trade recommendations.</returns>
    public static async Task<List<Recommendation>> AnalyzeAsync(List<CoinGecko.Coin> coins)
    {
        return [.. (await GetRecommendationsAsync(coins)).Where(rec => rec.Coin is not Constants.Ethereum)];
    }
    
    private static async Task<List<Recommendation>> GetRecommendationsAsync(List<CoinGecko.Coin> coins)
    {
        try
        {
            return await new GPTRecommender().NextAsync(coins);
        }
        catch (Exception exception)
        {
            // Fallback to the default recommendation algorithm.
            // This generally means we were unable to connect to OpenAI or have insufficient quota.

            WARN(exception, "Recommendations from OpenAI unavailable, falling back to default recommender.");

            return await new DefaultRecommender().NextAsync(coins);
        }
    }
}