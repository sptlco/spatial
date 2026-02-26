// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Compute;
using Spatial.Networking;
using Spatial.Simulation;
using System.Reflection;

namespace Spatial;

/// <summary>
/// Constant values used throughout the system.
/// </summary>
internal class Constants
{
    /// <summary>
    /// The name of the database collection that stores counters.
    /// </summary>
    public const string CounterCollectionName = "counters";

    /// <summary>
    /// The name of the module's log file.
    /// </summary>
    public static string LogFilePath = $"{Assembly.GetEntryAssembly()?.GetName().Name ?? "Spatial"}.log";

    /// <summary>
    /// The maximum number of components that can be registered.
    /// </summary>
    public const int MaxComponents = 128;

    /// <summary>
    /// The size of a <see cref="Chunk"/> in bytes.
    /// </summary>
    public const int ChunkSize = 1024 * 64;

    /// <summary>
    /// The name of an <see cref="Agent"/> thread.
    /// </summary>
    public const string AgentThreadName = "Spatial Agent";

    /// <summary>
    /// The maximum number of times an agent will yield.
    /// </summary>
    public const int AgentMaxYields = 100;

    /// <summary>
    /// The maximum size of the <see cref="Job"/> pool.
    /// </summary>
    public const int MaxPoolSize = ushort.MaxValue;

    /// <summary>
    /// The size of a <see cref="Connection"/>, in bytes.
    /// </summary>
    public const int ConnectionSize = 1024 * 16;

    /// <summary>
    /// The default name of the database.
    /// </summary>
    public const string DefaultDatabaseUrl = "mongodb://mongo:27017";

    /// <summary>
    /// The default name of the database.
    /// </summary>
    public const string DefaultDatabaseName = "spatial";

    /// <summary>
    /// The size of a cryptographic keystream table.
    /// </summary>
    public const int KeystreamSize = 512;

    /// <summary>
    /// The name of the log database collection.
    /// </summary>
    public const string LogCollectionName = "logs";

    /// <summary>
    /// The path to the system's configuration override file.
    /// </summary>
    public const string OverridePath = "appsettings.Override.json";

    /// <summary>
    /// The name of the interval cache group.
    /// </summary>
    public const string Intervals = "Intervals";

    /// <summary>
    /// The directory containing static files.
    /// </summary>
    public const string StaticFilePath = "wwwroot";

    /// <summary>
    /// The name of the metric key field.
    /// </summary>
    public const string MetricKey = "Name";

    /// <summary>
    /// Log property names.
    /// </summary>
    public static class Properties
    {
        /// <summary>
        /// A trace identifier.
        /// </summary>
        public const string TraceId = "TraceId";
    }

    /// <summary>
    /// Constant URI schemes.
    /// </summary>
    public static class UriSchemes
    {
        /// <summary>
        /// The HTTP URI scheme.
        /// </summary>
        public const string Http = "http";

        /// <summary>
        /// The HTTPS URI scheme.
        /// </summary>
        public const string Https = "https";

        /// <summary>
        /// The socket URI scheme.
        /// </summary>
        public const string Socket = "socket";
    }

    /// <summary>
    /// Constant API addresses.
    /// </summary>
    public static class API
    {
        /// <summary>
        /// Get coin data from Coin Gecko.
        /// </summary>
        /// <param name="coin">The coin whose data to get.</param>
        /// <returns>The Coin Gecko coin data API.</returns>
        public static string Coin(string coin) => $"https://api.coingecko.com/api/v3/coins/{coin}";

        /// <summary>
        /// Get market data from Coin Gecko.
        /// </summary>
        /// <param name="coins">The coins whose data to get.</param>
        /// <returns>The Coin Gecko market data API.</returns>
        public static string Markets(params string[] coins) => $"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd{(coins.Length > 0 ? "&ids=" : "")}{string.Join(",", coins)}";
    }

    /// <summary>
    /// Constant smart contract addresses.
    /// </summary>
    public static class Contracts
    {
        /// <summary>
        /// The Uniswap Router's contract address.
        /// </summary>
        public const string UniswapV2Router02 = "0x7a250d5630B4cF539739dF2C5dAcb4c659F2488D";

        /// <summary>
        /// The Multicall contract address.
        /// </summary>
        public const string Multicall = "0xcA11bde05977b3631167028862bE2a173976CA11";

        /// <summary>
        /// The Chainlink ETH/USD price feed.
        /// </summary>
        public const string CHAINLINK_ETH_USD = "0x5f4ec3df9cbd43714fe2740f5e3616155c5b8419";
    }

    /// <summary>
    /// Constant smart contract ABI definitions.
    /// </summary>
    public static class ABI
    {
        /// <summary>
        /// The ERC20 token ABI.
        /// </summary>
        public const string ERC20 = "https://gist.githubusercontent.com/veox/8800debbf56e24718f9f483e1e40c35c/raw/f853187315486225002ba56e5283c1dba0556e6f/erc20.abi.json";

        /// <summary>
        /// The Uniswap V2 Router 02 ABI.
        /// </summary>
        public const string UniswapV2Router02 = "https://gist.githubusercontent.com/cundiffd/c34a6778425295c8a4dafcaecf018f6b/raw/61f91032a0cf54b8216d602fdd3989b18c121437/uniswap.abi.json";

        /// <summary>
        /// The Multicall contract ABI.
        /// </summary>
        public const string Multicall = "https://gist.githubusercontent.com/cundiffd/4ea872974d516ef82e29cc68119dd4a8/raw/91f02b45ff992c8f69230dcded4c903761b5abd0/multicall.abi.json";
    }

    /// <summary>
    /// Constant smart contract function names.
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// A function that lets you query an ERC20 token balance.
        /// </summary>
        public const string BalanceOf = "balanceOf";

        /// <summary>
        /// A function that lets you aggregate multiple function calls into one.
        /// </summary>
        public const string Aggregate = "aggregate";

        /// <summary>
        /// Authorize the spending of an ERC20 token.
        /// </summary>
        public const string Approve = "approve";

        /// <summary>
        /// A function gets a token's decimal count for unit conversion.
        /// </summary>
        public const string Decimals = "decimals";

        /// <summary>
        /// A function that lets you swap Ethereum for ERC20 tokens.
        /// </summary>
        public const string SwapExactETHForTokens = "swapExactETHForTokens";

        /// <summary>
        /// A function that lets you swap ERC20 tokens for Ethereum.
        /// </summary>
        public const string SwapExactTokensForETH = "swapExactTokensForETH";

        /// <summary>
        /// A function that lets you get expected output token amounts from a swap.
        /// </summary>
        public const string GetAmountsOut = "getAmountsOut";
    }

    /// <summary>
    /// Authorization policy names.
    /// </summary>
    public static class Policies
    {
        /// <summary>
        /// The default RBAC policy.
        /// </summary>
        public const string RBAC = "RBAC";
    }
}

/// <summary>
/// Constant variable names.
/// </summary>
public static partial class Variables
{
    /// <summary>
    /// The name of the session variable.
    /// </summary>
    public const string Session = "Session";
}