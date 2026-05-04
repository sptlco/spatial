// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Identity.Authorization;
using Stripe;

namespace Spatial.Cloud.API;

/// <summary>
/// A <see cref-"Controller"/> for Stripe product functions.
/// </summary>
[Path("products")]
public class ProductController : Controller
{
    /// <summary>
    /// Get a list of products.
    /// </summary>
    /// <returns>A list of products.</returns>
    [GET]
    [Authorize]
    public async Task<List<Product>> ListProductsAsync()
    {
        return (await Logistics.Stripe.CreateClient().Products.ListAsync()).Data;
    }
}