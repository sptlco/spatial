// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Persistence;

/// <summary>
/// Specifies how frequently metrics are grouped into buckets.
/// </summary>
public enum Granularity
{
    /// <summary>
    /// Data is recorded frequently.
    /// </summary>
    Seconds,

    /// <summary>
    /// Data is recoreded less frequently.
    /// </summary>
    Minutes,

    /// <summary>
    /// Data is recoreded much less frequently.
    /// </summary>
    Hours
}