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
            Model = Logistics.Stripe.CreateClient().Products.Get(asset.Model)
        });
    }

    /// <summary>
    /// Create a new <see cref="Asset"/>.
    /// </summary>
    /// <param name="options">Configurable options for the <see cref="Asset"/>.</param>
    /// <returns>An <see cref="Asset"/>.</returns>
    /// <exception cref="BadRequest">Thrown if the associated product does not exist.</exception>
    [POST]
    [Authorize(Scope.Assets.Create)]
    public async Task<AssetView> CreateAssetAsync([Body] CreateAssetOptions options)
    {
        try
        {
            var stripe = Logistics.Stripe.CreateClient();
            var product = await stripe.Products.GetAsync(options.Model);

            var asset = new Asset {
                Metadata = options.Metadata,
                Model = options.Model,
                Type = options.Type,
                Quantity = options.Quantity,
                Location = options.Location,
                Variants = options.Variants
            };

            await asset.StoreAsync();

            return new AssetView {
                Asset = asset,
                Model = product
            };
        }
        catch (StripeException)
        {
            throw new BadRequest("The product does not exist.");
        }
    }

    /// <summary>
    /// Update an <see cref="Asset"/>.
    /// </summary>
    /// <param name="asset">The updated <see cref="Asset"/>.</param>
    /// <returns>The updated <see cref="Asset"/>.</returns>
    [PATCH]
    [Authorize(Scope.Assets.Update)]
    public async Task<Asset> UpdateAssetAsync([Body] Asset asset)
    {
        return asset.Save();
    }
}