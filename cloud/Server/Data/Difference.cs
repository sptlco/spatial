// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data;

/// <summary>
/// A difference between two states.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Difference<T>
{
    /// <summary>
    /// The elements that were added.
    /// </summary>
    public List<T> Added { get; set; }

    /// <summary>
    /// The elements that were removed.
    /// </summary>
    public List<T> Removed { get; set; }
}