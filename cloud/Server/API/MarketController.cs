// Copyright © Spatial Corporation. All rights reserved.

using Nethereum.Web3;
using Spatial.Blockchain;
using Spatial.Blockchain.Helpers;
using Spatial.Cloud.Data.Scopes;
using Spatial.Identity.Authorization;

namespace Spatial.Cloud.API;

/// <summary>
/// A <see cref="Controller"/> for market functions.
/// </summary>
[Path("market")]
public class MarketController : Controller
{
    /// <summary>
    /// Fetch the current market data.
    /// </summary>
    /// <returns>The current market data.</returns>
    [GET]
    [Authorize(Scope.Market.Fetch)]
    public async Task<CoinGecko.Coin> FetchAsync()
    {
        return (await CoinGecko.GetMarketsAsync(["ethereum"]))[0];
    }

    /// <summary>
    /// Get the current Ethereum balance.
    /// </summary>
    /// <returns>The current Ethereum balance.</returns>
    [GET]
    [Path("balance")]
    [Authorize(Scope.Market.Fetch)]
    public async Task<string> GetBalanceAsync()
    {
        return Web3.Convert.FromWei(await Ethereum.CreateClient().GetBalanceAsync()).ToString();
    }
}