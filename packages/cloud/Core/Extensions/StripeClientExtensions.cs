// Copyright © Spatial Corporation. All rights reserved.

using Stripe;

namespace Spatial.Extensions;


/// <summary>
/// Extension methods for <see cref="StripeClient"/>.
/// </summary>
public static class StripeClientExtensions
{
    extension(StripeClient _)
    {
        /// <summary>
        /// Create a new <see cref="StripeClient"/>.
        /// </summary>
        public static V1Services Create()
        {
            return new StripeClient(Application.Current.Configuration.Stripe.Key).V1;
        }
    }
}