// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Cloud.Data.Assets;

/// <summary>
/// A physical or digital good.
/// </summary>
[Collection("assets")]
public class Asset : Resource
{
    /// <summary>
    /// The asset's <see cref="AssetType"/>.
    /// </summary>
    public AssetType Type { get; set; }

    /// <summary>
    /// A unique code classifying the <see cref="Asset"/>.
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// The asset's <see cref="Data.Version"/>.
    /// </summary>
    public Version Version { get; set; } = new Version(1.0);

    /// <summary>
    /// A unique code shared by assets produced under the same conditions.
    /// </summary>
    public string Lot { get; set; }

    /// <summary>
    /// The asset's Stripe product identifier.
    /// </summary>
    public string Product { get; set; }

    /// <summary>
    /// For <see cref="AssetType.Physical"/> goods, the <see cref="Address"/> where the <see cref="Asset"/> is currently held.
    /// </summary>
    public Address? Location { get; set; } = null;

    /// <summary>
    /// A number indicating precisely how much of the <see cref="Asset"/> is currently held.
    /// </summary>
    public double Quantity { get; set; } = 1.0;
}