// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Blockchain.Helpers;
using Spatial.Intelligence;

namespace Spatial.Cloud.Systems.Banking.Engines;

/// <summary>
/// A <see cref="IRecommender"/> that communicates with ChatGPT as if 
/// it were a financial assistant for trade advice.
/// </summary>
internal class GPTRecommender : IRecommender
{
    /// <summary>
    /// Generate a list of trade recommendations.
    /// </summary>
    /// <param name="coins">Market data for each coin.</param>
    /// <returns>A list of trade recommendations./returns>
    public async Task<List<Recommendation>> NextAsync(List<CoinGecko.Coin> coins)
    {
        var response = await ChatGPT.GenerateAsync<Response>(
            instructions: @"
                You are a financial assistant.
                Return ONLY a JSON array of recommendations.
                Each element must have:

                - Coin (string): The coin's identifier.
                - Action (string): (""Buy"" or ""Sell"").
                - Size (number): Fraction of ETH balance (buy) or coin balance (sell).

                Example output: {
                    ""Recommendations"": [
                        { ""Coin"": ""chainlink"", ""Action"": ""Buy"", ""Size"": 0.15 },
                        { ""Coin"": ""tether"", ""Action"": ""Sell"", ""Size"": 0.35 }
                    ]
                }
            ",
            input: new { Coins = coins });
            
        return response.Recommendations;
    }

    /// <summary>
    /// A response from ChatGPT.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// A list of recommendations.
        /// </summary>
        public List<Recommendation> Recommendations { get; set; }
    }
}