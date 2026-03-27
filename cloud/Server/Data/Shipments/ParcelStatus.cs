// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Shipments;

/// <summary>
/// Indicates the current status of a <see cref="Parcel"/>.
/// </summary>
public enum ParcelStatus
{
    /// <summary>
    /// The <see cref="Parcel"/> was processed.
    /// </summary>
    Processed,

    /// <summary>
    /// The <see cref="Parcel"/> was shipped.
    /// </summary>
    Shipped,

    /// <summary>
    /// The <see cref="Parcel"/> is in transit.
    /// </summary>
    Moving,

    /// <summary>
    /// The <see cref="Parcel"/> was delivered.
    /// </summary>
    Delivered
}