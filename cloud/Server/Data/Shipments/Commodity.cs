// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Shipments;

/// <summary>
/// A good being shipped.
/// </summary>
public class Commodity
{
    /// <summary>
    /// The good's unit identifier.
    /// </summary>
    public string Unit { get; set; }

    /// <summary>
    /// The amount of the good being shipped.
    /// </summary>
    public int Quantity { get; set; }
}