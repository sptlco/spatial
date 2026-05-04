// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Assets;

/// <summary>
/// Configurable options for a new asset.
/// </summary>
public class CreateAssetOptions : CreateResourceOptions
{
    /// <summary>
    /// A unique code classifying the asset.
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// The asset's <see cref="AssetType"/>.
    /// </summary>
    public AssetType Type { get; set; }

    /// <summary>
    /// How much of this asset is being registered. Defaults to 1.
    /// </summary>
    public double Quantity { get; set; } = 1.0;

    /// <summary>
    /// For <see cref="AssetType.Physical"/> goods, where the asset is currently held.
    /// </summary>
    public Address? Location { get; set; }

    /// <summary>
    /// Configurable options for the <see cref="Asset"/>.
    /// </summary>
    public Dictionary<string, string> Variants { get; set; } = [];
}