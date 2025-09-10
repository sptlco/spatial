// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Blockchain.Helpers;
using System.Text.Json.Serialization;

namespace Spatial.Cloud.Systems.Banking;

/// <summary>
/// A trade <see cref="Recommendation"/> generator.
/// </summary>
internal static class Recommender
{
    /// <summary>
    /// Generate a trade <see cref="Recommendation"/>.
    /// </summary>
    /// <param name="coin">The coin to act on.</param>
    /// <param name="watchlist">A list of coins of interest.</param>
    /// <returns>A trade <see cref="Recommendation"/>.</returns>
    public static async Task<Recommendation> NextAsync(CoinGecko.Coin coin, List<CoinGecko.Coin> watchlist)
    {
        var eth = watchlist.FirstOrDefault(c => string.Equals(c.Id, "ethereum", StringComparison.OrdinalIgnoreCase));
        var ethBalance = eth?.Balance ?? 0.0M;
        var coinBalance = coin.Balance;

        var recommendation = await OpenAI.GenerateAsync<Recommendation>(
            instructions: @"
                You are a financial assistant that returns exactly one JSON object (no extra text).
                Analyze the provided coin and watchlist and produce a trading score and trade size for the coin:
                { ""Score"": <double>, ""Size"": <decimal> }

                Score: double, -5.0 .. +5.0 (negative=SELL, positive=BUY), -1.0 .. +1.0 = HOLD.
                Size: decimal, non-negative, coin units.
                For BUY: Size (ETH) < ETH_BALANCE (Swap ETH for the coin).
                For SELL: Size (COINS) < COIN_BALANCE (Swap the coin for ETH).
                Scale Size proportionally with magnitude of Score.
                Example: { ""Score"": ""2.35"", ""Size"": ""0.25"" }
            ",
            input: new {
                Coin = coin,
                Watchlist = watchlist,
                ETH_BALANCE = ethBalance,
                COIN_BALANCE = coinBalance
            });

        recommendation.Size = (recommendation.Score = Math.Clamp(recommendation.Score, -5.0, 5.0)) switch {
            >= 1.0 => Math.Clamp(recommendation.Size, 0.0M, ethBalance),
            <= -1.0 => Math.Clamp(recommendation.Size, 0.0M, coinBalance),
            _ => 0.0M
        };

        return recommendation;
    }

    /// <summary>
    /// A trade recommendation.
    /// </summary>
    public class Recommendation : OpenAI.Response
    {
        /// <summary>
        /// The recommended trade score.
        /// </summary>
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public double Score { get; set; }

        /// <summary>
        /// The recommended trade size.
        /// </summary>
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal Size { get; set; }
    }
}