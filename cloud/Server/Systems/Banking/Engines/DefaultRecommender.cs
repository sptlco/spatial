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
        var ethereum = coins.First(coin => coin.Id == Constants.Ethereum);
        var recommendations = new List<Recommendation>();

        foreach (var coin in coins)
        {
            if (coin.Id == Constants.Ethereum || coin.TotalVolume < Constants.vL)
            {
                continue;
            }

            var change = coin.PriceChangePercentage24H;

            if (change > 3.0D)
            {
                // Upward momentum.
                // Sell some of the coin.

                recommendations.Add(new Recommendation {
                    Coin = coin.Id,
                    Action = TradeAction.Sell,
                    Size = Math.Min(1.0D, change / 50.0D),
                });
            }
            else if (change < -3.0D)
            {
                // Downward momentum.
                // Buy some of the coin.

                recommendations.Add(new Recommendation {
                    Coin = coin.Id,
                    Action = TradeAction.Buy,
                    Size = Math.Min(0.2D, Math.Abs(change) / 50.0D),
                });
            }
        }

        return await Task.FromResult(recommendations);
    }
}