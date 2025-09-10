// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Blockchain;
using Spatial.Blockchain.Helpers;
using Spatial.Cloud.Models.Banking;
using System.Collections.Concurrent;

namespace Spatial.Cloud.Systems.Banking;

/// <summary>
/// A market data analyzer for ERC20 tokens.
/// </summary>
internal static class Analyzer
{
    /// <summary>
    /// Analyze a list of tokens.
    /// </summary>
    /// <param name="watchlist">The watchlist to analyze.</param>
    /// <returns>An <see cref="Analysis"/>.</returns>
    public static async Task<List<Analysis>> AnalyzeAsync(Dictionary<string, string?> watchlist)
    {
        var ethereum = Ethereum.GetOrCreateClient();
        var data = new ConcurrentBag<Analysis>();
        var coins = await CoinGecko.GetMarketsAsync([.. watchlist.Keys]);

        foreach (var coin in coins)
        {
            coin.Address = watchlist[coin.Id];
            coin.Balance = !string.IsNullOrEmpty(coin.Address) ? await ethereum.GetERC20BalanceAsync(coin.Address) : await ethereum.GetBalanceAsync();
        }

        foreach (var coin in coins)
        {
            if (string.IsNullOrEmpty(coin.Address))
            {
                continue;
            }

            var analysis = new Analysis(coin);
            var recommendation = Recommender.NextAsync(coin, coins).GetAwaiter().GetResult();

            analysis.Model = recommendation.Model;
            analysis.Score = recommendation.Score;
            analysis.Size = recommendation.Size;

            INFO("Analyzed {Coin} using {Model}, score: {Score:F8} size: {Size:F8}.", coin.Symbol.ToUpper(), analysis.Model, analysis.Score, analysis.Size);

            data.Add(analysis);
        }

        return [.. data];
    }
}