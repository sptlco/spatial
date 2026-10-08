// Copyright © Spatial Corporation. All rights reserved.

using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spatial.Persistence;

/// <summary>
/// A domain fact that occurred within the system.
/// </summary>
[Collection("events", TTL = Expiration.Year)]
public class Event : Resource
{
    /// <summary>
    /// The event's name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The time the event occurred.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Record a new <see cref="Event"/>.
    /// </summary>
    /// <param name="name">The event's name.</param>
    /// <param name="metadata">Contextual data about the event.</param>
    /// <returns>The recorded <see cref="Event"/>.</returns>
    public static Event Record(string name, object? metadata = null)
    {
        return Resource<Event>.StoreOne(Create(name, metadata));
    }

    /// <summary>
    /// Record a new <see cref="Event"/>.
    /// </summary>
    /// <param name="name">The event's name.</param>
    /// <param name="metadata">Contextual data about the event.</param>
    /// <returns>The recorded <see cref="Event"/>.</returns>
    public static Task<Event> RecordAsync(string name, object? metadata = null)
    {
        return Resource<Event>.StoreOneAsync(Create(name, metadata));
    }

    /// <summary>
    /// Record several new events.
    /// </summary>
    /// <param name="events">Events to write.</param>
    public static void RecordMany(Dictionary<string, object?> events)
    {
        Resource<Event>.StoreMany(events.Select(kvp => Create(kvp.Key, kvp.Value)));
    }

    /// <summary>
    /// Record several new events.
    /// </summary>
    /// <param name="events">Events to write.</param>
    public static Task RecordManyAsync(Dictionary<string, object?> events)
    {
        return Resource<Event>.StoreManyAsync(events.Select(kvp => Create(kvp.Key, kvp.Value)));
    }

    /// <summary>
    /// Read events by name.
    /// </summary>
    /// <param name="name">The event's name.</param>
    /// <param name="from">The starting period.</param>
    /// <param name="to">The ending period.</param>
    /// <param name="limit">The maximum number of events to read.</param>
    /// <returns>The matched events.</returns>
    public static List<Event> Read(
        string name,
        DateTime? from = null,
        DateTime? to = null,
        int? limit = null)
    {
        return Aggregate(name, null, from, to, limit).ToList();
    }

    /// <summary>
    /// Read events by name.
    /// </summary>
    /// <param name="name">The event's name.</param>
    /// <param name="from">The starting period.</param>
    /// <param name="to">The ending period.</param>
    /// <param name="limit">The maximum number of events to read.</param>
    /// <returns>The matched events.</returns>
    public static Task<List<Event>> ReadAsync(
        string name,
        DateTime? from = null,
        DateTime? to = null,
        int? limit = null)
    {
        return Aggregate(name, null, from, to, limit).ToListAsync();
    }

    /// <summary>
    /// Read events by name and metadata.
    /// </summary>
    /// <param name="name">The event's name.</param>
    /// <param name="metadata">Contextual data to match.</param>
    /// <param name="from">The starting period.</param>
    /// <param name="to">The ending period.</param>
    /// <param name="limit">The maximum number of events to read.</param>
    /// <returns>The matched events.</returns>
    public static List<Event> Read(
        string name,
        Dictionary<string, string> metadata,
        DateTime? from = null,
        DateTime? to = null,
        int? limit = null)
    {
        return Aggregate(name, metadata, from, to, limit).ToList();
    }

    /// <summary>
    /// Read events by name and metadata.
    /// </summary>
    /// <param name="name">The event's name.</param>
    /// <param name="metadata">Contextual data to match.</param>
    /// <param name="from">The starting period.</param>
    /// <param name="to">The ending period.</param>
    /// <param name="limit">The maximum number of events to read.</param>
    /// <returns>The matched events.</returns>
    public static Task<List<Event>> ReadAsync(
        string name,
        Dictionary<string, string> metadata,
        DateTime? from = null,
        DateTime? to = null,
        int? limit = null)
    {
        return Aggregate(name, metadata, from, to, limit).ToListAsync();
    }

    private static IAsyncCursor<Event> Aggregate(
        string name,
        Dictionary<string, string>? metadata,
        DateTime? from = null,
        DateTime? to = null,
        int? limit = null)
    {
        from ??= DateTime.UnixEpoch;
        to ??= DateTime.UtcNow;

        var filter = new BsonDocument
        {
            { nameof(Name), name },
            { nameof(Timestamp), new BsonDocument { { "$gte", from.Value }, { "$lte", to.Value } } }
        };

        if (metadata is not null)
        {
            foreach (var pair in metadata)
            {
                filter[$"{nameof(Metadata)}.{pair.Key}"] = pair.Value;
            }
        }

        var pipeline = new[]
        {
            new BsonDocument("$match", filter),
            new BsonDocument("$sort", new BsonDocument(nameof(Timestamp), 1))
        };

        if (limit is not null)
        {
            pipeline = [.. pipeline, new BsonDocument("$limit", limit)];
        }

        return Resource<Event>.Collection.Aggregate<Event>(pipeline);
    }

    private static Event Create(string name, object? metadata)
    {
        var options = new JsonSerializerOptions {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        var meta = new Dictionary<string, string>();

        if (metadata is not null)
        {
            foreach (var pair in JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(metadata, options)) ?? [])
            {
                meta[pair.Key] = pair.Value;
            }
        }

        return new Event {
            Name = name,
            Metadata = meta,
            Timestamp = DateTime.UtcNow
        };
    }
}