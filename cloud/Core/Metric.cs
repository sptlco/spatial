// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;
using System.Text.Json;

namespace Spatial;

/// <summary>
/// A time-stamped measurement of a specific attribute within the system.
/// </summary>
[Collection("metrics",
    TimeSeries = true,
    TimeField = nameof(Timestamp),
    MetaField = nameof(Metadata),
    Granularity = Granularity.Seconds,
    TTL = Expiration.Year)]
public class Metric : Resource
{
    /// <summary>
    /// The metric's data point(s).
    /// </summary>
    public Dictionary<string, decimal> Value { get; set; } = [];
 
    /// <summary>
    /// The time the metric's value occurred.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static Metric Write(string name, object? value, object? metadata = null)
    {
        return Resource<Metric>.Store(new Metric {
            Value = JsonSerializer.Deserialize<Dictionary<string, decimal>>(JsonSerializer.Serialize(value))!
        });
    }
}