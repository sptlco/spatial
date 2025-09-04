// Copyright © Spatial Corporation. All rights reserved.

using System.Numerics;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Spatial.Networking;

namespace Spatial.Blockchain;

/// <summary>
/// A means of interaction with Ethereum networks.
/// </summary>
public class Ethereum
{
    private static Ethereum? _instance;

    private readonly Account _account;
    private readonly Web3 _web3;
    private readonly Http _http;

    private Ethereum()
    {
        _account = new Account(Configuration.Current.Ethereum.Key);
        _web3 = new Web3(_account, Configuration.Current.Ethereum.Url);
        _http = new Http();
    }

    /// <summary>
    /// Create a new <see cref="Ethereum"/> client, or get an existing one.
    /// </summary>
    /// <returns>An <see cref="Ethereum"/> client.</returns>
    public static Ethereum GetOrCreateClient()
    {
        return _instance ??= new Ethereum();
    }

    /// <summary>
    /// Get the system's Ethereum balance.
    /// </summary>
    /// <returns>The system's current Ethereum balance.</returns>
    public async Task<BigInteger> GetBalanceAsync()
    {
        return (await _web3.Eth.GetBalance.SendRequestAsync(_account.Address)).Value;
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
        var target = _web3.Eth.GetContract(Download(abi), contract).GetFunction(function);
        var gas = await target.EstimateGasAsync(input);

        return await target.CallAsync<TResult>(_account.Address, gas, null, input);
    }

    /// <summary>
    /// Send a transaction to a smart contract.
    /// </summary>
    /// <param name="abi">The contract's ABI; a URL, file path, or source code.</param>
    /// <param name="contract">The contract's address.</param>
    /// <param name="function">The name of the function to send the transaction to.</param>
    /// <param name="input">The transaction's input parameters.</param>
    /// <returns>Receipt of the transaction.</returns>
    public async Task<Receipt> SendTransactionAsync(
        string abi,
        string contract,
        string function,
        params object[] input)
    {
        var target = _web3.Eth.GetContract(Download(abi), contract).GetFunction(function);
        var gas = await target.EstimateGasAsync(input);
        var receipt = await target.SendTransactionAndWaitForReceiptAsync(_account.Address, gas, null, null, input);

        return new Receipt(receipt.TransactionHash);
    }

    private string Download(string contract)
    {
        if (Uri.TryCreate(contract, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
        {
            return _http.CreateClient().GetStringAsync(uri).GetAwaiter().GetResult();
        }

        return File.Exists(contract) ? File.ReadAllText(contract) : contract;
    }
}