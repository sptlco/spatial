// Copyright © Spatial Corporation. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace Spatial.Billing;

/// <summary>
/// Configurable options for <see cref="Stripe"/>.
/// </summary>
public class StripeConfiguration
{
    /// <summary>
    /// A <see cref="Stripe"/> API key.
    /// </summary>
    public string Key { get; set; }
}