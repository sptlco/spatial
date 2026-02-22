// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Helpers;
using System.Text.Json;

namespace Spatial.Blockchain.Helpers;

/// <summary>
/// Helpers methods for CoinGecko.
/// </summary>
public static class CoinGecko
{
    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Get ERC20 coin data.
    /// </summary>
    /// <param name="coin">A coin identifier.</param>
    /// <returns>The coin's data.</returns>
    public static async Task<Coin> GetCoinAsync(string coin)
    {
        return JsonSerializer.Deserialize<Coin>(await Http.GetOrCreateClient().GetStringAsync(Constants.API.Coin(coin)), _jsonOptions)!;
    }

    /// <summary>
    /// Get market data for ERC20 coins.
    /// </summary>
    /// <param name="coins">An array of coin identifiers.</param>
    /// <returns>A list of coins.</returns>
    public static async Task<List<Coin>> GetMarketsAsync(params string[] coins)
    {
        return JsonSerializer.Deserialize<List<Coin>>(await Http.GetOrCreateClient().GetStringAsync(Constants.API.Markets(coins)), _jsonOptions)!;
    }
}