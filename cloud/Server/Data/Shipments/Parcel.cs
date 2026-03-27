// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Cloud.Data.Shipments;

/// <summary>
/// A physical package containing goods. Parcels are the primary persisted entity;
/// logical shipments are derived by grouping on <see cref="Shipment"/>.
/// </summary>
[Collection("parcels")]
public class Parcel : Resource
{
    /// <summary>
    /// Groups this <see cref="Parcel"/> with others that belong to the same logical <see cref="Shipment"/>.
    /// </summary>
    public string Shipment { get; set; }

    /// <summary>
    /// The parcel's origin <see cref="Address"/>.
    /// </summary>
    public Address Origin { get; set; }

    /// <summary>
    /// The parcel's destination <see cref="Address"/>.
    /// </summary>
    public Address Destination { get; set; }

    /// <summary>
    /// The postal carrier responsible for the <see cref="Parcel"/>.
    /// </summary>
    public string Carrier { get; set; }

    /// <summary>
    /// The tracking number issued for the <see cref="Parcel"/> by the <see cref="Carrier"/>.
    /// </summary>
    public string TrackingNumber { get; set; }

    /// <summary>
    /// The current status of the <see cref="Parcel"/>.
    /// </summary>
    public ParcelStatus Status { get; set; } = ParcelStatus.Processed;

    /// <summary>
    /// The weight of the <see cref="Parcel"/> in grams.
    /// </summary>
    public decimal Weight { get; set; }

    /// <summary>
    /// The parcel's <see cref="Dimensions"/>.
    /// </summary>
    public Dimensions Dimensions { get; set; } = new();

    /// <summary>
    /// The items contained in the <see cref="Parcel"/>.
    /// </summary>
    public List<Commodity> Units { get; set; } = [];
}