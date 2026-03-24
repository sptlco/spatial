// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Shipments;

/// <summary>
/// Configurable options for a new shipment.
/// </summary>
public class CreateShipmentOptions : CreateResourceOptions
{
    /// <summary>
    /// The origin address.
    /// </summary>
    public Address From { get; set; }

    /// <summary>
    /// The destination address.
    /// </summary>
    public Address To { get; set; }
}