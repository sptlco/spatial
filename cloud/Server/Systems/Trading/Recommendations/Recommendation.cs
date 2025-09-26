// Copyright © Spatial Corporation. All rights reserved.

using System.Text.Json.Serialization;

namespace Spatial.Cloud.Systems.Trading.Recommendations;

/// <summary>
/// A trade recommendation.
/// </summary>
public class Recommendation
{
    /// <summary>
    /// The coin's identifier.
    /// </summary>
    public string Coin { get; set; }

    /// <summary>
    /// The recommended action.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TradeAction Action { get; set; }

    /// <summary>
    /// The size of the trade as a fraction, also encoding the strength/confidence of the trade.
    /// </summary>
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public double Size { get; set; }
}

/// <summary>
/// Specifies the action the system should take in regards to an ERC20 coin.
/// </summary>
public enum TradeAction
{
    /// <summary>
    /// Buy the coin.
    /// </summary>
    Buy,

    /// <summary>
    /// Sell the coin.
    /// </summary>
    Sell
}