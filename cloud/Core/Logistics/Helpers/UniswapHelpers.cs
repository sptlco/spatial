// Copyright © Spatial Corporation. All rights reserved.

using Nethereum.RPC.Eth.DTOs;
using System.Numerics;

namespace Spatial.Logistics.Helpers;

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
    public static async Task<TransactionReceipt> SwapExactETHForTokensAsync(
        BigInteger amountIn,
        BigInteger amountOutMin,
        string[] path,
        string to,
        uint deadline)
    {
        return await Ethereum.CreateClient().SendTransactionAsync(
            abi: Constants.ABI.UniswapV2Router02,
            contract: Constants.Contracts.UniswapV2Router02,
            function: Constants.Functions.SwapExactETHForTokens,
            value: amountIn,
            input: [amountOutMin, path, to, deadline]);
    }

    /// <summary>
    /// Estimate the gas required to swap an exact amount of ETH for as many output tokens as possible, along the route determined by the <paramref name="path"/>.
    /// </summary>
    /// <param name="amountIn">The amount of ETH to send.</param>
    /// <param name="amountOutMin">The minimum amount of output tokens that must be received for the transaction not to revert.</param>
    /// <param name="path">An array of token addresses.</param>
    /// <param name="to">Recipient of the output tokens.</param>
    /// <param name="deadline">Unix timestamp after which the transaction will revert.</param>
    /// <returns>The estimated gas units required to perform the transaction.</returns>
    public static async Task<BigInteger> EstimateSwapExactETHForTokensGasAsync(
        BigInteger amountIn,
        BigInteger amountOutMin,
        string[] path,
        string to,
        uint deadline)
    {
        return await Ethereum.CreateClient().EstimateGasAsync(
            abi: Constants.ABI.UniswapV2Router02,
            contract: Constants.Contracts.UniswapV2Router02,
            function: Constants.Functions.SwapExactETHForTokens,
            value: amountIn,
            input: [amountOutMin, path, to, deadline]);
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
    public static async Task<TransactionReceipt> SwapExactTokensForETHAsync(
        BigInteger amountIn,
        BigInteger amountOutMin,
        string[] path,
        string to,
        uint deadline)
    {
        return await Ethereum.CreateClient().SendTransactionAsync(
            abi: Constants.ABI.UniswapV2Router02,
            contract: Constants.Contracts.UniswapV2Router02,
            function: Constants.Functions.SwapExactTokensForETH,
            input: [amountIn, amountOutMin, path, to, deadline]);
    }

     /// <summary>
    /// Swap an exact amount of tokens for as many tokens, along the route determined by the <paramref name="path"/>.
    /// </summary>
    /// <param name="amountIn">The amount of input tokens to send.</param>
    /// <param name="amountOutMin">The minimum amount of output tokens that must be received for the transaction not to revert.</param>
    /// <param name="path">An array of token addresses.</param>
    /// <param name="to">Recipient of the tokens.</param>
    /// <param name="deadline">Unix timestamp after which the transaction will revert.</param>
    /// <returns>The input token amount and all subsequent output token amounts.</returns>
    public static async Task<TransactionReceipt> SwapExactTokensForTokensAsync(
        BigInteger amountIn,
        BigInteger amountOutMin,
        string[] path,
        string to,
        uint deadline)
    {
        return await Ethereum.CreateClient().SendTransactionAsync(
            abi: Constants.ABI.UniswapV2Router02,
            contract: Constants.Contracts.UniswapV2Router02,
            function: Constants.Functions.SwapExactTokensForTokens,
            input: [amountIn, amountOutMin, path, to, deadline]);
    }

    /// <summary>
    /// Given an input asset amount and an array of token addresses, calculate all subsequent maximum 
    /// output token amounts.
    /// </summary>
    /// <param name="amountIn">The amount of input tokens.</param>
    /// <param name="path">An array of token addresses.</param>
    /// <returns>All output token amounts.</returns>
    public static async Task<List<BigInteger>> GetAmountsOutAsync(BigInteger amountIn, string[] path)
    {
        return await Ethereum.CreateClient().CallAsync<List<BigInteger>>(
            abi: Constants.ABI.UniswapV2Router02,
            contract: Constants.Contracts.UniswapV2Router02,
            function: Constants.Functions.GetAmountsOut,
            input: [amountIn, path]);
    }
}