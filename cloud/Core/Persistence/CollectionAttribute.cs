// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Persistence;

/// <summary>
/// Specifies the collection of a <see cref="Resource"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class CollectionAttribute : Attribute
{
    /// <summary>
    /// Create a new <see cref="CollectionAttribute"/>.
    /// </summary>
    /// <param name="name">The name of the collection.</param>
    public CollectionAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// The name of the collection.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Whether or not the collection is a time-series collection.
    /// </summary>
    public bool TimeSeries { get; set; } = false;

    /// <summary>
    /// The name of the field containing a timestamp.
    /// </summary>
    public string? TimeField { get; set; }

    /// <summary>
    /// The name of the field containing the time series data.
    /// </summary>
    public string? MetaField { get; set; }

    /// <summary>
    /// The granularity of the time series data.
    /// </summary>
    public Granularity Granularity { get; set; } = Granularity.Seconds;

    /// <summary>
    /// The amount of time documents in the collection will live for.
    /// </summary>
    public TimeSpan TTL { get; set; }
}


