// Copyright © Spatial Corporation. All rights reserved.

using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
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
    /// Create a new <see cref="Ethereum"/> client.
    /// </summary>
    /// <returns>An <see cref="Ethereum"/> client.</returns>
    public static Ethereum CreateClient()
    {
        return new Ethereum();
    }

    /// <summary>
    /// Authorize a designated smart contract to spend a quantity of tokens.
    /// </summary>
    /// <param name="contract">The token's smart contract.</param>
    /// <param name="spender">The address of the permitted smart contract or any Ethereum address.</param>
    /// <param name="amount">The amount of tokens that can be spent.</param>
    public async Task ApproveAsync(string contract, string spender, BigInteger amount)
    {
        await SendTransactionAsync(
            abi: Constants.ABI.ERC20,
            contract: contract,
            function: Constants.Functions.Approve,
            input: [spender, amount]);
    }

    /// <summary>
    /// Get ERC20 coin details.
    /// </summary>
    /// <param name="coins">A list of ERC20 coins.</param>
    /// <returns>The coins' details.</returns>
    public async Task<Dictionary<string, (BigInteger Balance, byte Decimals)>> GetERC20DetailsAsync(params string[] coins)
    {
        var results = await MulticallAsync(coins.SelectMany<string, Call>(coin => [
            new Call { Target = coin, CallData = new BalanceOfFunction { Owner = _account.Address }.GetCallData() },
            new Call { Target = coin, CallData = new DecimalsFunction().GetCallData() }
        ]));

        var decoder = new FunctionCallDecoder();
        var details = new Dictionary<string, (BigInteger Balance, byte Decimals)>();
        var balance = new Parameter("uint256");
        var decimals = new Parameter("uint8");

        for (var i = 0; i < coins.Length; i++)
        {
            var index = i * 2;

            details[coins[i]] = (
                Balance: decoder.DecodeSimpleTypeOutput<BigInteger>(balance, results[index].ToHex(true)),
                Decimals: decoder.DecodeSimpleTypeOutput<byte>(decimals, results[index + 1].ToHex(true)));
        }

        return details;
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
    /// <param name="value">An amount of ETH to send along with the transaction.</param>
    /// <param name="input">The transaction's input parameters.</param>
    /// <returns>Receipt of the transaction.</returns>
    public async Task<string> SendTransactionAsync(
        string abi,
        string contract,
        string function,
        BigInteger? value = null,
        params object[] input)
    {
        var target = _web3.Eth.GetContract(Download(abi), contract).GetFunction(function);
        var wei = value.HasValue ? new HexBigInteger(value.Value) : null;
        var gas = await target.EstimateGasAsync(_account.Address, null, wei, input);

        return await target.SendTransactionAsync(_account.Address, gas, wei, input);
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