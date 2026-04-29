// Copyright © Spatial Corporation. All rights reserved.

using Stripe;

namespace Spatial.Cloud.Data.Assets;

/// <summary>
/// A detailed view of an <see cref="Assets.Asset"/>.
/// </summary>
public class AssetView
{
    /// <summary>
    /// The requested <see cref="Asset"/>.
    /// </summary>
    public Asset Asset { get; set; }

    /// <summary>
    /// The Stripe <see cref="Stripe.Product"/> associated with the <see cref="Asset"/>. 
    /// </summary>
    public Product Product { get; set; }
}