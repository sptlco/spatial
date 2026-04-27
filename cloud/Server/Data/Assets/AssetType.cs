// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Assets;

/// <summary>
/// Classifies the type of an <see cref="Asset"/>.
/// </summary>
public enum AssetType
{
    /// <summary>
    /// The <see cref="Asset"/> is a physical good.
    /// </summary>
    Physical,

    /// <summary>
    /// The <see cref="Asset"/> is a digital good.
    /// </summary>
    Digital
}