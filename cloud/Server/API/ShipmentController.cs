// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Scopes;
using Spatial.Cloud.Data.Shipments;
using Spatial.Extensions;
using Spatial.Identity.Authorization;
using Spatial.Persistence;

namespace Spatial.Cloud.API;

/// <summary>
/// A controller for shipment functions.
/// </summary>
[Path("shipments")]
public class ShipmentController : Controller
{
    /// <summary>
    /// Create a new <see cref="Shipment"/>.
    /// </summary>
    /// <param name="options">Configurable options for the <see cref="Shipment"/>.</param>
    /// <returns>A <see cref="Shipment"/>.</returns>
    [POST]
    [Authorize(Scope.Shipments.Create)]
    public async Task<Shipment> CreateShipmentAsync([Body] CreateShipmentOptions options)
    {
        var shipment = new Shipment {
            From = options.From,
            To = options.To
        };

        return shipment.Store();
    }

    /// <summary>
    /// List all shipments.
    /// </summary>
    /// <returns>A list of <see cref="Shipment"/>.</returns>
    [GET]
    [Authorize(Scope.Shipments.List)]
    public async Task<List<Shipment>> ListShipmentsAsync()
    {
        return Resource<Shipment>.List();
    }

    /// <summary>
    /// Update a shipment.
    /// </summary>
    /// <param name="shipment">The shipment to update.</param>
    /// <returns>The updated shipment.</returns>
    [PATCH]
    [Authorize(Scope.Shipments.Update)]
    public async Task<Shipment> UpdateShipmentAsync([Body] Shipment shipment)
    {
        return shipment.Save();
    }

    /// <summary>
    /// Delete a shipment.
    /// </summary>
    /// <param name="shipment">The shipment to delete.</param>
    [DELETE]
    [Path("{shipment}")]
    [Authorize(Scope.Shipments.Delete)]
    public async Task DeleteShipmentAsync(string shipment)
    {
        Resource<Shipment>.RemoveOne(s => s.Id == shipment);
    }
}