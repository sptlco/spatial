// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud;

/// <summary>
/// Constant values used by the cloud server.
/// </summary>
internal static class Constants
{
    /// <summary>
    /// The maximum number of tokens the global portfolio can hold.
    /// </summary>
    public const byte MaxTokens = 30;

    /// <summary>
    /// The preferred GPT model.
    /// </summary>
    public const string GPT = "gpt-5";

    /// <summary>
    /// The name of the system.
    /// </summary>
    public const string Spatial = "spatial";

    /// <summary>
    /// The name of the native asset.
    /// </summary>
    public const string Ethereum = "ethereum";

    /// <summary>
    /// The wrapped Ethereum (WETH) contract address.
    /// </summary>
    public const string WETH = "0xc02aaa39b223fe8d0a0e5c4f27ead9083c756cc2";

    /// <summary>
    /// The Uniswap Router's contract address.
    /// </summary>
    public const string UniswapV2Router02 = "0x7a250d5630B4cF539739dF2C5dAcb4c659F2488D";

    /// <summary>
    /// The coin volume at which the coin is considered liquid.
    /// </summary>
    public const decimal vL = 1000000;

    /// <summary>
    /// The minimum number of hours between trades executed by the <see cref="Systems.Banking.Trader"/>.
    /// </summary>
    public const double MinTradeIntervalHours = 0.5;

    /// <summary>
    /// Constant interval names.
    /// </summary>
    public static class Intervals
    {
        /// <summary>
        /// The name of the trade interval.
        /// </summary>
        public const string Trade = "Trader.Trade";
    }
}