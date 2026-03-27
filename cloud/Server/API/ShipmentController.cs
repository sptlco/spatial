// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Scopes;
using Spatial.Cloud.Data.Shipments;
using Spatial.Extensions;
using Spatial.Identity.Authorization;
using Spatial.Persistence;

namespace Spatial.Cloud.API;

/// <summary>
/// A controller for shipment functions. Shipments are logical groupings of
/// <see cref="Parcel"/> documents — this controller aggregates them on the fly.
/// </summary>
[Path("shipments")]
public class ShipmentController : Controller
{
    /// <summary>
    /// Create a new <see cref="Shipment"/>.
    /// </summary>
    /// <param name="options">Configurable options for the <see cref="Shipment"/>.</param>
    /// <returns>A new <see cref="Shipment"/>.</returns>
    [POST]
    [Authorize(Scope.Shipments.Create)]
    public async Task<Shipment> CreateShipmentAsync([Body] CreateShipmentOptions options)
    {
        var shipment = new UID();
        var parcels = options.Packages.Select(parcel => new Parcel {
            Shipment = shipment,
            Origin = parcel.Origin,
            Destination = parcel.Destination,
            Carrier = parcel.Carrier,
            TrackingNumber = parcel.TrackingNumber,
            Status = parcel.Status,
            Weight = parcel.Weight,
            Dimensions = parcel.Dimensions,
            Units = parcel.Units
        });

        await parcels.StoreAsync();

        return new Shipment {
            Id = shipment,
            Packages = [..parcels]
        };
    }

    /// <summary>
    /// Get a <see cref="Shipment"/>.
    /// </summary>
    /// <returns>A <see cref="Shipment"/> projection.</returns>
    [GET]
    [Path("{shipment}")]
    [Authorize(Scope.Shipments.Get)]
    public async Task<Shipment> GetShipmentAsync(string shipment)
    {
        return Project(shipment, Resource<Parcel>.List(p => p.Shipment == shipment));
    }

    /// <summary>
    /// List all shipments by fetching every <see cref="Parcel"/> and grouping
    /// them by <see cref="Parcel.Shipment"/>.
    /// </summary>
    /// <returns>A list of <see cref="Shipment"/> projections.</returns>
    [GET]
    [Authorize(Scope.Shipments.List)]
    public async Task<List<Shipment>> ListShipmentsAsync()
    {
        return [.. Resource<Parcel>
            .List()
            .GroupBy(p => p.Shipment)
            .Select(g => Project(g.Key, [.. g]))];
    }

    /// <summary>
    /// Update a parcel within a shipment.
    /// </summary>
    /// <param name="parcel">The parcel to update.</param>
    /// <returns>The updated <see cref="Parcel"/>.</returns>
    [PATCH]
    [Path("parcels/{parcel}")]
    [Authorize(Scope.Shipments.Update_Parcel)]
    public async Task<Parcel> UpdateParcelAsync([Body] Parcel parcel)
    {
        return parcel.Save();
    }

    /// <summary>
    /// Delete a <see cref="Shipment"/>.
    /// </summary>
    /// <param name="shipment">A <see cref="Shipment"/> identifier.</param>
    [DELETE]
    [Path("{shipment}")]
    [Authorize(Scope.Shipments.Delete)]
    public async Task DeleteShipmentAsync(string shipment)
    {
        Resource<Parcel>.RemoveMany(p => p.Shipment == shipment);
    }

    /// <summary>
    /// Delete a <see cref="Parcel"/>.
    /// </summary>
    /// <param name="parcel">A <see cref="Parcel"/> identifier.</param>
    [DELETE]
    [Path("parcels/{parcel}")]
    [Authorize(Scope.Shipments.Delete_Parcel)]
    public async Task DeleteParcelAsync(string parcel)
    {
        Resource<Parcel>.RemoveOne(p => p.Id == parcel);
    }

    private static Shipment Project(string id, List<Parcel> parcels)
    {
        return new Shipment
        {
            Id = id,
            Packages = parcels
        };
    }
}