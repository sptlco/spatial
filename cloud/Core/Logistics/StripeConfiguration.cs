// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Logistics;

/// <summary>
/// Configurable options for <see cref="Stripe"/>.
/// </summary>
public class StripeConfiguration
{
    /// <summary>
    /// A <see cref="Stripe"/> API key.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Configurable options for Stripe webhooks.
    /// </summary>
    public WebhookConfiguration Webhooks { get; set; }

    /// <summary>
    /// Configurable options for Stripe webhooks.
    /// </summary>
    public class WebhookConfiguration
    {
        /// <summary>
        /// A Stripe webhook endpoint signing secret.
        /// </summary>
        public string Secret { get; set; }
    }
}