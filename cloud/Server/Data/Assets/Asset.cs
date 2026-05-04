// Copyright © Spatial Corporation. All rights reserved.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Spatial.Persistence;

namespace Spatial.Cloud.Data.Assets;

/// <summary>
/// A physical or digital good.
/// </summary>
[Collection("assets")]
public class Asset : Resource
{
    /// <summary>
    /// The asset's Stripe product identifier.
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// The asset's <see cref="AssetType"/>.
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    public AssetType Type { get; set; }

    /// <summary>
    /// A number indicating precisely how much of the <see cref="Asset"/> is currently held.
    /// </summary>
    public double Quantity { get; set; } = 1.0;

    /// <summary>
    /// For <see cref="AssetType.Physical"/> goods, the <see cref="Address"/> where the <see cref="Asset"/> is currently held.
    /// </summary>
    public Address? Location { get; set; } = null;

    /// <summary>
    /// Configurable options for the <see cref="Asset"/>.
    /// </summary>
    public Dictionary<string, string> Variants { get; set; } = [];
}