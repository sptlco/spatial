// Copyright © Spatial Corporation. All rights reserved.

using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.Hex.HexConvertors.Extensions;
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
    public async Task<BigInteger> GetBalanceAsync()
    {
        return await GetBalanceAsync(_account.Address);
    }

    /// <summary>
    /// Get an Ethereum balance.
    /// </summary>
    /// <param name="address">A wallet address.</param>
    /// <returns>The wallet's Ethereum balance.</returns>
    public async Task<BigInteger> GetBalanceAsync(string address)
    {
        return await _web3.Eth.GetBalance.SendRequestAsync(address);
    }

    /// <summary>
    /// Get aggregated coin balances.
    /// </summary>
    /// <param name="coins">A list of ERC20 coins.</param>
    /// <returns>The coins' balances.</returns>
    public async Task<Dictionary<string, BigInteger>> GetERC20BalancesAsync(params string[] coins)
    {
        var calls = coins.Select(coin => new Call {
            Target = coin,
            CallData = new BalanceOfFunction { Owner = _account.Address }.GetCallData()
        });

        var results = await MulticallAsync(calls);
        var balances = new Dictionary<string, BigInteger>();
        var decoder = new FunctionCallDecoder();
        var parameter = new Parameter("uint256");

        for (var i = 0; i < coins.Length; i++)
        {
            balances[coins[i]] = decoder.DecodeSimpleTypeOutput<BigInteger>(parameter, results[i].ToHex(true));
        }

        return balances;
    }

    /// <summary>
    /// Get a token balance.
    /// </summary>
    /// <param name="token">An ERC20 token address.</param>
    /// <returns>The wallet's <paramref name="token"/> balance.</returns>
    public async Task<BigInteger> GetERC20BalanceAsync(string token)
    {
        return await GetERC20BalanceAsync(token, _account.Address);
    }

    /// <summary>
    /// Get a token balance.
    /// </summary>
    /// <param name="token">An ERC20 token address.</param>
    /// <param name="address">A wallet address.</param>
    /// <returns>The wallet's <paramref name="token"/> balance.</returns>
    public async Task<BigInteger> GetERC20BalanceAsync(string token, string address)
    {
        return await CallAsync<BigInteger>(
            abi: Constants.ABI.ERC20,
            contract: token,
            function: Constants.Functions.BalanceOf,
            input: [address]);
    }

    /// <summary>
    /// Batch multiple calls using the Multicall contract.
    /// </summary>
    /// <param name="calls">A list of function calls.</param>
    /// <returns>The aggregated results of the function calls.</returns>
    public async Task<List<byte[]>> MulticallAsync(IEnumerable<Call> calls)
    {
        var result = await _web3.Eth
            .GetContract(Download(Constants.ABI.Multicall), Constants.Contracts.Multicall)
            .GetFunction(Constants.Functions.Aggregate)
            .CallAsync<AggregateOutputDTO>([(object[]) [.. calls.Select(c => new object[] { c.Target, c.CallData })]]);

        return result.ReturnData;
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

    /// <summary>
    /// Aggregated function call results.
    /// </summary>
    public class AggregatedResults
    {
        /// <summary>
        /// The block that executed the function calls.
        /// </summary>
        public BigInteger Block { get; set; }

        /// <summary>
        /// The results of each function call.
        /// </summary>
        public List<byte[]> Results { get; set; }
    }
}