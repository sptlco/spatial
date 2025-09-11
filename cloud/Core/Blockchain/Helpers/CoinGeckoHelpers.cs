// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Networking.Helpers;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spatial.Blockchain.Helpers;

/// <summary>
/// Helpers methods for CoinGecko.
/// </summary>
public static class CoinGecko
{
    /// <summary>
    /// Get ERC20 coin data.
    /// </summary>
    /// <param name="coin">A coin identifier.</param>
    /// <returns>The coin's data.</returns>
    public static async Task<Coin> GetCoinAsync(string coin)
    {
        return JsonSerializer.Deserialize<Coin>(await Http.GetOrCreateClient().GetStringAsync(Constants.API.Coin(coin)), new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        })!;
    }

    /// <summary>
    /// Get market data for ERC20 coins.
    /// </summary>
    /// <param name="coins">An array of coin identifiers.</param>
    /// <returns>A list of coins.</returns>
    public static async Task<List<Coin>> GetMarketsAsync(params string[] coins)
    {
        return JsonSerializer.Deserialize<List<Coin>>(await Http.GetOrCreateClient().GetStringAsync(Constants.API.Markets(coins)), new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        })!;
    }

    /// <summary>
    /// An ERC20 coin.
    /// </summary>
    public class Coin
    {
        /// <summary>
        /// The coin's contract address.
        /// </summary>
        [JsonPropertyName("contract_address")]
        public string Address { get; set; }

        /// <summary>
        /// The coin's identifier.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// The token's symbol.
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        /// <summary>
        /// The token's name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The token's current price.
        /// </summary>
        [JsonPropertyName("current_price")]
        public decimal Price { get; set; }

        /// <summary>
        /// The coin's balance.
        /// </summary>
        public BigInteger Balance { get; set; }

        /// <summary>
        /// The token's market capitalization.
        /// </summary>
        [JsonPropertyName("market_cap")]
        public decimal MarketCap { get; set; }

        /// <summary>
        /// The coin's market capitalization rank.
        /// </summary>
        [JsonPropertyName("market_cap_rank")]
        public int? MarketCapRank { get; set; }

        /// <summary>
        /// The coin's fully diluted valuation.
        /// </summary>
        [JsonPropertyName("fully_diluted_valuation")]
        public decimal FullyDilutedValuation { get; set; }

        /// <summary>
        /// The token's volume (24H).
        /// </summary>
        [JsonPropertyName("total_volume")]
        public decimal TotalVolume { get; set; }

        /// <summary>
        /// The token's highest price over the last 24 hours.
        /// </summary>
        [JsonPropertyName("high_24h")]
        public decimal High24H { get; set; }

        /// <summary>
        /// The token's lowest price over the last 24 hours.
        /// </summary>
        [JsonPropertyName("low_24h")]
        public decimal Low24H { get; set; }

        /// <summary>
        /// The token's change in price over the last 24 hours.
        /// </summary>
        [JsonPropertyName("price_change_24h")]
        public decimal PriceChange24H { get; set; }

        /// <summary>
        /// The token's change in price over the last 24 hours.
        /// </summary>
        [JsonPropertyName("price_change_percentage_24h")]
        public double PriceChangePercentage24H { get; set; }

        /// <summary>
        /// The token's change in market capitalization over the last 24 hours.
        /// </summary>
        [JsonPropertyName("market_cap_change_24h")]
        public decimal MarketCapChange24H { get; set; }

        /// <summary>
        /// The token's change in market capitalization over the last 24 hours.
        /// </summary>
        [JsonPropertyName("market_cap_change_percentage_24h")]
        public double MarketCapChangePercentage24H { get; set; }

        /// <summary>
        /// The token's circulating supply.
        /// </summary>
        [JsonPropertyName("circulating_supply")]
        public decimal CirculatingSupply { get; set; }

        /// <summary>
        /// The token's total supply.
        /// </summary>
        [JsonPropertyName("total_supply")]
        public decimal TotalSupply { get; set; }

        /// <summary>
        /// The token's maximum supply.
        /// </summary>
        [JsonPropertyName("max_supply")]
        public decimal? MaxSupply { get; set; }

        /// <summary>
        /// The token's all-time-high price.
        /// </summary>
        [JsonPropertyName("ath")]
        public decimal ATH { get; set; }

        /// <summary>
        /// The token's change in its all-time-high price.
        /// </summary>
        [JsonPropertyName("ath_change_percentage")]
        public double ATHChangePercentage { get; set; }

        /// <summary>
        /// The date the token reached its all-time-high price.
        /// </summary>
        [JsonPropertyName("ath_date")]
        public DateTime ATHDate { get; set; }

        /// <summary>
        /// The token's all-time-low price.
        /// </summary>
        [JsonPropertyName("atl")]
        public decimal ATL { get; set; }

        /// <summary>
        /// The token's change in its all-time-low price.
        /// </summary>
        [JsonPropertyName("atl_change_percentage")]
        public double ATLChangePercentage { get; set; }

        /// <summary>
        /// The date the token reached its all-time-low price.
        /// </summary>
        [JsonPropertyName("atl_date")]
        public DateTime ATLDate { get; set; }

        /// <summary>
        /// The coin's return on investment.
        /// </summary>
        [JsonPropertyName("roi")]
        public Return? ROI { get; set; }

        /// <summary>
        /// The last time the coin was updated.
        /// </summary>
        [JsonPropertyName("last_updated")]
        public DateTime Updated { get; set; }

        /// <summary>
        /// A return on investment.
        /// </summary>
        public class Return
        {
            /// <summary>
            /// The multiplier of the return.
            /// </summary>
            [JsonPropertyName("times")]
            public decimal Times { get; set; }

            /// <summary>
            /// The currency used to calculate the ROI.
            /// </summary>
            [JsonPropertyName("currency")]
            public string Currency { get; set; }

            /// <summary>
            /// The percent gain/loss.
            /// </summary>
            [JsonPropertyName("percentage")]
            public double Percentage { get; set; }
        }
    }
}