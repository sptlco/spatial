// Copyright © Spatial Corporation. All rights reserved.

using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace Spatial.Exchange;

/// <summary>
/// A means of interaction with Ethereum networks.
/// </summary>
public class Ethereum
{
    private readonly Account _account;
    private readonly Web3 _web3;

    /// <summary>
    /// Create a new <see cref="Ethereum"/>.
    /// </summary>
    public Ethereum()
    {
        _account = new Account(Environment.PrivateKey);
        _web3 = new Web3(_account, Environment.RPCUrl);
    }

    // ...
}