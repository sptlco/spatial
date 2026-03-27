// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Shipments;

/// <summary>
/// A logical grouping of <see cref="Parcel"/> documents sharing a common  shipment identifier. 
/// Not persisted directly — derived at query time by grouping parcels on <see cref="Parcel.Shipment"/>.
/// </summary>
public class Shipment
{
    /// <summary>
    /// The shared identifier across all <see cref="Parcel"/> documents in this shipment.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The parcels that make up this shipment.
    /// </summary>
    public List<Parcel> Packages { get; set; } = [];
}