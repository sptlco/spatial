// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Blockchain.Helpers;

/// <summary>
/// Helper methods for Uniswap.
/// </summary>
public static class Uniswap
{
    /// <summary>
    /// Swap an exact amount of ETH for as many output tokens as possible, along the route determined by the <paramref name="path"/>.
    /// </summary>
    /// <param name="amountIn">The amount of ETH to send.</param>
    /// <param name="amountOutMin">The minimum amount of output tokens that must be received for the transaction not to revert.</param>
    /// <param name="path">An array of token addresses.</param>
    /// <param name="to">Recipient of the output tokens.</param>
    /// <param name="deadline">Unix timestamp after which the transaction will revert.</param>
    /// <returns>The input token amount and all subsequent output token amounts.</returns>
    public static async Task<uint[]> SwapExactETHForTokensAsync(
        uint amountIn,
        uint amountOutMin,
        string[] path,
        string to,
        uint deadline)
    {
        return await Ethereum.GetOrCreateClient().CallAsync<uint[]>(
            abi: Constants.ABI.UniswapV2Router02,
            contract: Constants.Contracts.UniswapV2Router02,
            function: Constants.Functions.SwapExactTokensForETH,
            input: [amountIn, amountOutMin, path, to, deadline]);
    }

    /// <summary>
    /// Swap an exact amount of tokens for as much ETH as possible, along the route determined by the <paramref name="path"/>.
    /// </summary>
    /// <param name="amountIn">The amount of input tokens to send.</param>
    /// <param name="amountOutMin">The minimum amount of output tokens that must be received for the transaction not to revert.</param>
    /// <param name="path">An array of token addresses.</param>
    /// <param name="to">Recipient of the ETH.</param>
    /// <param name="deadline">Unix timestamp after which the transaction will revert.</param>
    /// <returns>The input token amount and all subsequent output token amounts.</returns>
    public static async Task<uint[]> SwapExactTokensForETHAsync(
        uint amountIn,
        uint amountOutMin,
        string[] path,
        string to,
        uint deadline)
    {
        return await Ethereum.GetOrCreateClient().CallAsync<uint[]>(
            abi: Constants.ABI.UniswapV2Router02,
            contract: Constants.Contracts.UniswapV2Router02,
            function: Constants.Functions.SwapExactTokensForETH,
            input: [amountIn, amountOutMin, path, to, deadline]);
    }

    /// <summary>
    /// Swap an exact amount of input tokens for as many output tokens as possible, along the route determined by the <paramref name="path"/>.
    /// </summary>
    /// <param name="amountIn">The amount of input tokens to send.</param>
    /// <param name="amountOutMin">The minimum amount of output tokens that must be received for the transaction not to revert.</param>
    /// <param name="path">An array of token addresses.</param>
    /// <param name="to">Recipient of the output tokens.</param>
    /// <param name="deadline">Unix timestamp after which the transaction will revert.</param>
    /// <returns>The input token amount and all subsequent output token amounts.</returns>
    public static async Task<uint[]> SwapExactTokensForTokensAsync(
        uint amountIn,
        uint amountOutMin,
        string[] path,
        string to,
        uint deadline)
    {
        return await Ethereum.GetOrCreateClient().CallAsync<uint[]>(
            abi: Constants.ABI.UniswapV2Router02,
            contract: Constants.Contracts.UniswapV2Router02,
            function: Constants.Functions.SwapExactTokensForTokens,
            input: [amountIn, amountOutMin, path, to, deadline]);
    }
}