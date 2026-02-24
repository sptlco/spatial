// Copyright © Spatial Corporation. All rights reserved.

using System.Text.Json;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Spatial.Extensions;
using Spatial.Persistence;

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

    /// <summary>
    /// Read aggregated metric data.
    /// </summary>
    /// <param name="name">The name of the metric to aggregate.</param>
    /// <param name="from">The starting period.</param>
    /// <param name="to">The ending period.</param>
    /// <param name="limit">The maximum number of data points to read.</param>
    /// <param name="resolution">The resolution of the aggregated data.</param>
    /// <returns>Aggregated metric data points.</returns>
    public static List<Metric> Read(
        string name, 
        DateTime? from = null,
        DateTime? to = null,
        int? limit = null,
        string? resolution = null)
    {
        return Aggregate(name, from, to, limit, resolution).ToList();
    }

    /// <summary>
    /// Read aggregated metric data.
    /// </summary>
    /// <param name="name">The name of the metric to aggregate.</param>
    /// <param name="from">The starting period.</param>
    /// <param name="to">The ending period.</param>
    /// <param name="limit">The maximum number of data points to read.</param>
    /// <param name="resolution">The resolution of the aggregated data.</param>
    /// <returns>Aggregated metric data points.</returns>
    public static Task<List<Metric>> ReadAsync(
        string name, 
        DateTime? from = null,
        DateTime? to = null,
        int? limit = null,
        string? resolution = null)
    {
        return Aggregate(name, from, to, limit, resolution).ToListAsync();
    }

    /// <summary>
    /// Store a new <see cref="Metric"/>.
    /// </summary>
    /// <param name="name">The name of the <see cref="Metric"/>.</param>
    /// <param name="value">The metric's value.</param>
    /// <param name="metadata">Contextual data about the <see cref="Metric"/>.</param>
    /// <returns>The stored <see cref="Metric"/>.</returns>
    public static Metric Write(string name, object value, object? metadata = null)
    {
        return Resource<Metric>.Store(Create(name, value, metadata));
    }

    /// <summary>
    /// Store a new <see cref="Metric"/>.
    /// </summary>
    /// <param name="name">The name of the <see cref="Metric"/>.</param>
    /// <param name="value">The metric's value.</param>
    /// <param name="metadata">Contextual data about the <see cref="Metric"/>.</param>
    /// <returns>The stored <see cref="Metric"/>.</returns>
    public static Task<Metric> WriteAsync(string name, object value, object? metadata = null)
    {
        return Resource<Metric>.StoreAsync(Create(name, value, metadata));
    }

    private static IAsyncCursor<Metric> Aggregate(
        string name, 
        DateTime? from = null,
        DateTime? to = null,
        int? limit = null,
        string? resolution = null)
    {
        from ??= DateTime.UnixEpoch;
        to ??= DateTime.UtcNow;

        var collection = Resource<Metric>.Collection;
        var filter = new BsonDocument {
            { $"{nameof(Metadata)}.{Constants.MetricKey}", name },
            { "Timestamp", new BsonDocument { { "$gte", from.Value }, { "$lte", to.Value } } }
        };

        if (string.IsNullOrEmpty(resolution))
        {
            var range = to.Value - from.Value;

            if (range > TimeSpan.FromDays(180))
            {
                resolution = "1d";
            }    
            else if (range > TimeSpan.FromDays(30))
            {
                resolution = "1h";
            }
            else if (range > TimeSpan.FromDays(7))
            {
                resolution = "15m";
            }
            else
            {
                resolution = "1m";
            }
        }

        var (unit, binSize) = resolution switch
        {
            "1m" => ("minute", 1),
            "5m" => ("minute", 5),
            "15m" => ("minute", 15),
            "1h" => ("hour", 1),
            "4h" => ("hour", 4),
            "1d" => ("day", 1),
            _ => ("minute", 1)
        };

        var pipeline = new[]
        {
            new BsonDocument("$match", filter),
            new BsonDocument("$sort", new BsonDocument("Timestamp", 1)),
            new BsonDocument("$group", new BsonDocument {
                {
                    "_id",
                    new BsonDocument("$dateTrunc", new BsonDocument
                    {
                        { "date", "$Timestamp" },
                        { "unit", unit },
                        { "binSize", binSize }
                    })
                },
                { "last", new BsonDocument("$last", "$$ROOT") }
            }),
            new BsonDocument("$replaceRoot", new BsonDocument("newRoot", "$last")),
            new BsonDocument("$sort", new BsonDocument("Timestamp", 1))
        };

        if (limit is not null)
        {
            pipeline = [..pipeline, new BsonDocument("$limit", limit)];
        }

        return collection.Aggregate<Metric>(pipeline);
    }

    private static Metric Create(string name, object value, object? metadata = null)
    {
        var meta = new Dictionary<string, string> { [Constants.MetricKey] = name };
        var options = new JsonSerializerOptions {
          DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,  
        };

        foreach (var pair in JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(metadata, options)) ?? [])
        {
            meta[pair.Key] = pair.Value;
        }

        return new Metric {
            Value = value as Dictionary<string, decimal> ?? JsonSerializer.Deserialize<Dictionary<string, decimal>>(JsonSerializer.Serialize(value, options)) ?? [],
            Metadata = meta,
            Timestamp = DateTime.UtcNow
        };
    }
}