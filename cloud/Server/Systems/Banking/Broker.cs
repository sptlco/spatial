// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Blockchain;
using Spatial.Blockchain.Helpers;
using System.Numerics;

namespace Spatial.Cloud.Systems.Banking;

/// <summary>
/// A trade execution agent.
/// </summary>
public static class Broker
{
    /// <summary>
    /// Buy an ERC20 coin.
    /// </summary>
    /// <param name="coin">The coin to buy.</param>
    /// <param name="size">The amount of ETH to swap for the coin.</param>
    /// <returns>The hash of the transaction.</returns>
    public static async Task<string> BuyAsync(CoinGecko.Coin coin, BigInteger size)
    {
        var ethereum = Ethereum.GetOrCreateClient();
        var path = new string[] { Constants.WETH, coin.Address };

        return await Uniswap.SwapExactETHForTokensAsync(
            amountIn: size,
            amountOutMin: await GetAmountOutMin(size, path),
            path: path,
            to: ethereum.Account.Address,
            deadline: GetDeadline());
    }

    /// <summary>
    /// Sell an ERC20 coin.
    /// </summary>
    /// <param name="coin">The coin to sell.</param>
    /// <param name="size">The amount of the coin to swap for ETH.</param>
    /// <returns>The hash of the transaction.</returns>
    public static async Task<string> SellAsync(CoinGecko.Coin coin, BigInteger size)
    {
        var ethereum = Ethereum.GetOrCreateClient();
        var path = new string[] { coin.Address, Constants.WETH };

        await ethereum.ApproveAsync(coin.Address, Constants.UniswapV2Router02, size);

        return await Uniswap.SwapExactTokensForETHAsync(
            amountIn: size,
            amountOutMin: await GetAmountOutMin(size, path),
            path: path,
            to: ethereum.Account.Address,
            deadline: GetDeadline());
    }

    private static uint GetDeadline() => (uint) DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds();

    private static async Task<BigInteger> GetAmountOutMin(BigInteger size, string[] path)
    {
        return (BigInteger)((double) (await Uniswap.GetAmountsOutAsync(size, path)).Last() * 0.99);
    }
}