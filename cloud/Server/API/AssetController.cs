// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Assets;
using Spatial.Cloud.Data.Scopes;
using Spatial.Extensions;
using Spatial.Identity.Authorization;
using Spatial.Persistence;
using Stripe;

namespace Spatial.Cloud.API;

/// <summary>
/// A <see cref="Controller"/> for <see cref="Asset"/> functions.
/// </summary>
[Path("assets")]
public class AssetController : Controller
{
    /// <summary>
    /// Get a list of assets.
    /// </summary>
    /// <returns>A list of assets.</returns>
    [GET]
    [Authorize(Scope.Assets.List)]
    public async Task<List<AssetView>> ListAssetsAsync()
    {
        return (await Resource<Asset>.ListAsync()).Map(asset => new AssetView {
            Asset = asset,
            Product = Logistics.Stripe.CreateClient().Products.Get(asset.Product)
        });
    }

    [POST]
    [Authorize(Scope.Assets.Create)]
    public async Task<AssetView> CreateAssetAsync([Body] CreateAssetOptions options)
    {
        try
        {
            var stripe = Logistics.Stripe.CreateClient();
            var product = await stripe.Products.GetAsync(options.Product);

            var asset = new Asset {
                Metadata = options.Metadata,
                Type = options.Type,
                Model = options.Model,
                Product = options.Product,
                Lot = options.Lot,
                Quantity = options.Quantity,
                Location = options.Location
            };

            await asset.StoreAsync();

            return new AssetView {
                Asset = asset,
                Product = product
            };
        }
        catch (StripeException)
        {
            throw new BadRequest("The product does not exist.");
        }
    }
}