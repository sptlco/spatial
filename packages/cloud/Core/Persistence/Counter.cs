// Copyright © Spatial Corporation. All rights reserved.

using MongoDB.Driver;

namespace Spatial.Persistence;

internal class Counter
{
    /// <summary>
    /// Gets or sets the unique name identifying this counter (e.g. the sequence name).
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current value of the counter.
    /// </summary>
    public uint Sequence { get; set; }
}

internal static class Counters
{
    private static IMongoCollection<Counter>? _collection;

    private static IMongoCollection<Counter> GetCollection(IMongoDatabase database)
    {
        return _collection ??= database.GetCollection<Counter>("counters");
    }

    /// <summary>
    /// Atomically increments and returns the next value of the named counter, creating it
    /// with an initial value of 1 if it does not already exist.
    /// </summary>
    /// <param name="database">The database containing the counters collection.</param>
    /// <param name="name">The unique name of the counter to increment.</param>
    /// <returns>The counter's new value after incrementing.</returns>
    public static uint Next(IMongoDatabase database, string name)
    {
        var filter = Builders<Counter>.Filter.Eq(c => c.Id, name);
        var update = Builders<Counter>.Update.Inc<uint>(c => c.Sequence, 1);
        var options = new FindOneAndUpdateOptions<Counter> {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        return GetCollection(database).FindOneAndUpdate(filter, update, options).Sequence;
    }

    /// <summary>
    /// Asynchronously and atomically increments and returns the next value of the named
    /// counter, creating it with an initial value of 1 if it does not already exist.
    /// </summary>
    /// <param name="database">The database containing the counters collection.</param>
    /// <param name="name">The unique name of the counter to increment.</param>
    /// <returns>A task that resolves to the counter's new value after incrementing.</returns>
    public static async Task<uint> NextAsync(IMongoDatabase database, string name)
    {
        var filter = Builders<Counter>.Filter.Eq(c => c.Id, name);
        var update = Builders<Counter>.Update.Inc<uint>(c => c.Sequence, 1);
        var options = new FindOneAndUpdateOptions<Counter> {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        return (await GetCollection(database).FindOneAndUpdateAsync(filter, update, options)).Sequence;
    }
}