// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Shipments;

/// <summary>
/// Physical dimensions of a <see cref="Parcel"/>.
/// </summary>
public class Dimensions
{
    /// <summary>
    /// The length of the <see cref="Parcel"/> in centimeters.
    /// </summary>
    public decimal Length { get; set; }

    /// <summary>
    /// The width of the <see cref="Parcel"/> in centimeters.
    /// </summary>
    public decimal Width { get; set; }

    /// <summary>
    /// The height of the <see cref="Parcel"/> in centimeters.
    /// </summary>
    public decimal Height { get; set; }
}