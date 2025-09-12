// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Cloud.Systems.Banking;

namespace Spatial.Cloud.Contracts.Systems.Banking;

/// <summary>
/// Configurable options for banking.
/// </summary>
internal class BankingConfiguration
{
    /// <summary>
    /// Configurable options for the <see cref="Banking.Trader"/>.
    /// </summary>
    [ValidateObjectMembers]
    public TraderConfiguration Trader { get; set; } = new TraderConfiguration();
}