// Copyright © Spatial Corporation. All rights reserved.

using Stripe;

namespace Spatial.Logistics;

/// <summary>
/// A means of interaction with Stripe.
/// </summary>
public class Stripe
{
    /// <summary>
    /// Create a new <see cref="Stripe"/> client.
    /// </summary>
    /// <returns></returns>
    public static V1Services CreateClient()
    {
        return new StripeClient(Configuration.Current.Stripe.Key).V1;
    }
}