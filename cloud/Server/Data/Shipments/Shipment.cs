// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Cloud.Data.Shipments;

/// <summary>
/// A shipment between two addresses.
/// </summary>
[Collection("shipments")]
public class Shipment : Resource
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