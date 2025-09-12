// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Cloud.Contracts.Systems.Banking;

namespace Spatial.Cloud.Contracts.Systems;

/// <summary>
/// Configurable options for cloud systems.
/// </summary>
internal class SystemConfiguration
{
    /// <summary>
    /// Configurable options for banking.
    /// </summary>
    [ValidateObjectMembers]
    public BankingConfiguration Banking { get; set; } = new BankingConfiguration();
}