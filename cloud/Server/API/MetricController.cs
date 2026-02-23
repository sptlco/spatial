// Copyright © Spatial Corporation. All rights reserved.

using MongoDB.Bson;
using MongoDB.Driver;
using Spatial.Cloud.Data.Metrics;
using Spatial.Cloud.Data.Scopes;
using Spatial.Extensions;
using Spatial.Identity.Authorization;
using Spatial.Persistence;

namespace Spatial.Cloud.API;

/// <summary>
/// A <see cref="Controller"/> for metric data.
/// </summary>
[Path("metrics")]
public class MetricController : Controller
{
    /// <summary>
    /// Reads data points for the specified metric.
    /// </summary>
    /// <param name="name">The metric's name.</param>
    /// <param name="from">Optional start timestamp (UTC).</param>
    /// <param name="to">Optional end timestamp (UTC).</param>
    /// <param name="limit">Optional maximum number of records to return.</param>
    /// <param name="resolution">Optional search resolution.</param>
    /// <returns>A list of matching <see cref="Metric"/> records.</returns>
    [GET]
    [Path("{name}")]
    [Authorize(Scope.Metrics.Read)]
    public async Task<List<Metric>> ReadAsync(
        string name,
        [Query] DateTime? from = null,
        [Query] DateTime? to = null,
        [Query] int? limit = null,
        [Query] string? resolution = null)
    {
        from ??= DateTime.UnixEpoch;
        to ??= DateTime.UtcNow;

        var collection = Resource<Metric>.Collection;
        var filterBuilder = Builders<Metric>.Filter;
        var filter = filterBuilder.Eq(m => m.Metadata[nameof(name)], name);

        filter &= filterBuilder.Gte(m => m.Timestamp, from.Value);
        filter &= filterBuilder.Lte(m => m.Timestamp, to.Value);

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
            new BsonDocument("$match", filter.Render(new RenderArgs<Metric> {
                DocumentSerializer = collection.DocumentSerializer,
                SerializerRegistry = collection.Settings.SerializerRegistry
            })),
            new BsonDocument("$sort", new BsonDocument("Timestamp", 1)),
            new BsonDocument("$group", new BsonDocument
            {
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
            new BsonDocument("$sort", new BsonDocument("Timestamp", 1)),
            new BsonDocument("$limit", limit ?? 5000)
        };

        return await collection.Aggregate<Metric>(pipeline).ToListAsync();
    }

    /// <summary>
    /// Create a new <see cref="Metric"/>.
    /// </summary>
    /// <param name="options">Configurable options for the <see cref="Metric"/>.</param>
    /// <returns>A <see cref="Metric"/>.</returns>
    [POST]
    [Authorize(Scope.Metrics.Write)]
    public async Task<Metric> WriteAsync([Body] WriteMetricOptions options)
    {
        var metric = new Metric {
            Metadata = options.Metadata,
            Value = options.Value
        };

        return metric.Store();
    }
}