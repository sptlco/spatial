// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Blockchain.Helpers;

namespace Spatial.Cloud.Systems.Banking.Engines;

/// <summary>
/// A <see cref="IRecommender"/> that communicates with ChatGPT as if 
/// it were a financial assistant for trade advice.
/// </summary>
internal class OpenAIRecommender : IRecommender
{
    /// <summary>
    /// Generate a list of trade recommendations.
    /// </summary>
    /// <param name="coins">Market data for each coin.</param>
    /// <returns>A list of trade recommendations./returns>
    public async Task<List<Recommendation>> NextAsync(List<CoinGecko.Coin> coins)
    {
        var response = await OpenAI.GenerateAsync<Response>(
            instructions: @"
                You are a financial assistant.
                Return ONLY a JSON array (no objects, no metadata, no explanations).
                Each array element must be a recommendation object with exactly these fields:

                - Coin (string): The coin's identifier.
                - Action (string): (""buy"" or ""sell"") (use lowercase, must exactly match).
                - Size (number): Trade size as a fraction of:
                    • If buying: ETH_BALANCE.
                    • If selling: The coin's balance.
                - Confidence (number): Confidence score from 0–100.

                IMPORTANT:
                - Balances are in WEI.
                - Don't generate a recommendation for native Ethereum.
                - Do not include any extra keys like Capacity, Item, or metadata.
                - The output must be valid JSON that matches List<Recommendation>.
                - Example output:
                [
                    { ""Coin"": ""ethereum"", ""Action"": ""hold"", ""Size"": 0, ""Confidence"": 70 },
                    { ""Coin"": ""chainlink"", ""Action"": ""buy"", ""Size"": 0.15, ""Confidence"": 65 }
                ]
            ",
            input: new {
                Coins = coins,
                ETH_BALANCE = coins.First(coin => coin.Id == Constants.Ethereum).Balance
            });
            
        return response.Recommendations;
    }

    /// <summary>
    /// A response from OpenAI.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// A list of recommendations.
        /// </summary>
        public List<Recommendation> Recommendations { get; set; }
    }
}