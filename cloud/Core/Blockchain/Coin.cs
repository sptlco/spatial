// Copyright © Spatial Corporation. All rights reserved.

using System.Text.Json.Serialization;

namespace Spatial.Blockchain;

/// <summary>
/// An ERC20 coin.
/// </summary>
public class Coin
{
    /// <summary>
    /// The coin's identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// The coin's symbol.
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    /// <summary>
    /// The coin's name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    /// <summary>
    /// The coin's displayed image.
    /// </summary>
    [JsonPropertyName("image")]
    public string Image { get; set; }

    /// <summary>
    /// The coin's current price.
    /// </summary>
    [JsonPropertyName("current_price")]
    public decimal Price { get; set; }

    /// <summary>
    /// The coin's current market cap.
    /// </summary>
    [JsonPropertyName("market_cap")]
    public decimal? MarketCap { get; set; }

    /// <summary>
    /// The coin's rank by market cap.
    /// </summary>
    [JsonPropertyName("market_cap_rank")]
    public decimal? MarketCapRank { get; set; }

    /// <summary>
    /// The coin's rank by market cap including rehypothecated tokens.
    /// </summary>
    [JsonPropertyName("market_cap_rank_with_rehypothecated")]
    public decimal? MarketCapRankWithRehypothecated { get; set; }

    /// <summary>
    /// The coin's fully diluted valuation (FDV).
    /// </summary>
    [JsonPropertyName("fully_diluted_valuation")]
    public decimal? FullyDilutedValuation { get; set; }

    /// <summary>
    /// The coin's total trading volume.
    /// </summary>
    [JsonPropertyName("total_volume")]
    public decimal? TotalVolume { get; set; }

    /// <summary>
    /// The coin's 24HR price high.
    /// </summary>
    [JsonPropertyName("high_24h")]
    public decimal? High24H { get; set; }

    /// <summary>
    /// The coin's 24H price low.
    /// </summary>
    [JsonPropertyName("low_24h")]
    public decimal? Low24H { get; set; }

    /// <summary>
    /// The coin's 24H price change.
    /// </summary>
    [JsonPropertyName("price_change_24h")]
    public decimal? PriceChange24H { get; set; }

    /// <summary>
    /// The coin's 24H price change percentage.
    /// </summary>
    [JsonPropertyName("price_change_percentage_24h")]
    public decimal? PriceChangePercentage24H { get; set; }

    /// <summary>
    /// The coin's 24H market cap change.
    /// </summary>
    [JsonPropertyName("market_cap_change_24h")]
    public decimal? MarketCapChange24H { get; set; }

    /// <summary>
    /// Thte coin's 24H market cap change percentage.
    /// </summary>
    [JsonPropertyName("market_cap_change_percentage_24h")]
    public decimal? MarketCapChangePercentage24H { get; set; }

    /// <summary>
    /// The coin's circulating supply.
    /// </summary>
    [JsonPropertyName("circulating_supply")]
    public decimal? CirculatingSupply { get; set; }

    /// <summary>
    /// The coin's total supply.
    /// </summary>
    [JsonPropertyName("total_supply")]
    public decimal? TotalSupply { get; set; }

    /// <summary>
    /// The coin's maximum supply.
    /// </summary>
    [JsonPropertyName("max_supply")]
    public decimal? MaxSupply { get; set; }

    /// <summary>
    /// The coin's all time high (ATH).
    /// </summary>
    [JsonPropertyName("ath")]
    public decimal? ATH { get; set; }

    /// <summary>
    /// The coin's all time high (ATH) change percentage.
    /// </summary>
    [JsonPropertyName("ath_change_percentage")]
    public decimal? ATHChangePercentage { get; set; }

    /// <summary>
    /// The coin's all time high (ATH) date.
    /// </summary>
    [JsonPropertyName("ath_date")]
    public DateTime? ATHDate { get; set; }

    /// <summary>
    /// The coin's all time low (ATL).
    /// </summary>
    [JsonPropertyName("atl")]
    public decimal? ATL { get; set; }

    /// <summary>
    /// The coin's all time low (ATL) change percentage.
    /// </summary>
    [JsonPropertyName("atl_change_percentage")]
    public decimal? ATLChangePercentage { get; set; }

    /// <summary>
    /// The coin's all time low (ATL) date.
    /// </summary>
    [JsonPropertyName("atl_date")]
    public DateTime? ATLDate { get; set; }

    /// <summary>
    /// The coin's return on investment.
    /// </summary>
    [JsonPropertyName("roi")]
    public Return? ROI { get; set; }

    /// <summary>
    /// The coin's last updated timestamp.
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