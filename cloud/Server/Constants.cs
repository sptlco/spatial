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