// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Blockchain;

/// <summary>
/// Record of an Ethereum transaction.
/// </summary>
/// <param name="Hash">The transaction's hash.</param>
public record struct Receipt(string Hash);