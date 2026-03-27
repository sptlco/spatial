// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Shipments;

/// <summary>
/// Configurable options for a new <see cref="Shipment"/>.
/// </summary>
public class CreateShipmentOptions
{
    /// <summary>
    /// The packages to ship.
    /// </summary>
    public List<CreateParcelOptions> Packages { get; set; } = [];
}