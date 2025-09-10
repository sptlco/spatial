// Copyright © Spatial Corporation. All rights reserved.

using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Spatial.Networking.Helpers;
using System.Numerics;

namespace Spatial.Blockchain;

/// <summary>
/// A means of interaction with Ethereum networks.
/// </summary>
public class Ethereum
{
    private static Ethereum? _instance;

    private readonly Account _account;
    private readonly Web3 _web3;

    private Ethereum()
    {
        _account = new Account(Configuration.Current.Ethereum.Key);
        _web3 = new Web3(_account, Configuration.Current.Ethereum.Endpoint);
    }

    /// <summary>
    /// The application's <see cref="Nethereum.Web3.Accounts.Account"/>.
    /// </summary>
    public Account Account => _account;

    /// <summary>
    /// Create a new <see cref="Ethereum"/> client, or get an existing one.
    /// </summary>
    /// <returns>An <see cref="Ethereum"/> client.</returns>
    public static Ethereum GetOrCreateClient()
    {
        return _instance ??= new Ethereum();
    }

    /// <summary>
    /// Get the Ethereum balance.
    /// </summary>
    /// <returns>The wallet's Ethereum balance.</returns>
    public async Task<decimal> GetBalanceAsync()
    {
        return await GetBalanceAsync(_account.Address);
    }

    /// <summary>
    /// Get an Ethereum balance.
    /// </summary>
    /// <param name="address">A wallet address.</param>
    /// <returns>The wallet's Ethereum balance.</returns>
    public async Task<decimal> GetBalanceAsync(string address)
    {
        return (decimal) (await _web3.Eth.GetBalance.SendRequestAsync(address)).Value / (decimal) 1e18;
    }

    /// <summary>
    /// Get a token balance.
    /// </summary>
    /// <param name="token">An ERC20 token address.</param>
    /// <returns>The wallet's <paramref name="token"/> balance.</returns>
    public async Task<decimal> GetERC20BalanceAsync(string token)
    {
        return await GetERC20BalanceAsync(token, _account.Address);
    }

    /// <summary>
    /// Get a token balance.
    /// </summary>
    /// <param name="token">An ERC20 token address.</param>
    /// <param name="address">A wallet address.</param>
    /// <returns>The wallet's <paramref name="token"/> balance.</returns>
    public async Task<decimal> GetERC20BalanceAsync(string token, string address)
    {
        var decimals = await CallAsync<int>(
            abi: Constants.ABI.ERC20,
            contract: token,
            function: Constants.Functions.Decimals);

        return (decimal) await CallAsync<BigInteger>(
            abi: Constants.ABI.ERC20,
            contract: token,
            function: Constants.Functions.BalanceOf,
            input: [address]) / (decimal) Math.Pow(10, decimals);
    }

    /// <summary>
    /// Call a smart contract's function.
    /// </summary>
    /// <typeparam name="TResult">The function's output type.</typeparam>
    /// <param name="abi">The contract's ABI; a URL, file path, or source code.</param>
    /// <param name="contract">The contract's address.</param>
    /// <param name="function">The name of the function to call.</param>
    /// <param name="input">The call's input parameters.</param>
    /// <returns>The result of the function call.</returns>
    public async Task<TResult> CallAsync<TResult>(
        string abi,
        string contract,
        string function,
        params object[] input)
    {
        return await _web3.Eth
            .GetContract(Download(abi), contract)
            .GetFunction(function)
            .CallAsync<TResult>(_account.Address, null, null, input);
    }

    /// <summary>
    /// Send a transaction to a smart contract.
    /// </summary>
    /// <param name="abi">The contract's ABI; a URL, file path, or source code.</param>
    /// <param name="contract">The contract's address.</param>
    /// <param name="function">The name of the function to send the transaction to.</param>
    /// <param name="input">The transaction's input parameters.</param>
    /// <returns>Receipt of the transaction.</returns>
    public async Task<string> SendTransactionAsync(
        string abi,
        string contract,
        string function,
        params object[] input)
    {
        var target = _web3.Eth.GetContract(Download(abi), contract).GetFunction(function);
        var gas = await target.EstimateGasAsync(input);

        return (await target.SendTransactionAndWaitForReceiptAsync(_account.Address, gas, null, null, input)).TransactionHash;
    }

    private string Download(string contract)
    {
        if (Uri.TryCreate(contract, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
        {
            return Http.GetOrCreateClient().GetStringAsync(uri).GetAwaiter().GetResult();
        }

        return File.Exists(contract) ? File.ReadAllText(contract) : contract;
    }
}