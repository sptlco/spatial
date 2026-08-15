// Copyright © Spatial Corporation. All rights reserved.

using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Spatial.Persistence;

/// <summary>
/// An object stored in the database.
/// </summary>
public class Resource
{
    /// <summary>
    /// The document's identification number.
    /// </summary>
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>
    /// The <see cref="DateTime"/> the <see cref="Resource"/> was created.
    /// </summary>
    public double Created { get; set; } = Time.Now;

    /// <summary>
    /// Arbitrary properties describing the item.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = [];
}

/// <summary>
/// A document stored in the database.
/// </summary>
public static class Resource<T> where T : Resource
{
    private static MongoClient _client;
    private static CollectionAttribute? _collection;
    private static bool _indexed;

    /// <summary>
    /// Create a new <see cref="Resource{T}"/>.
    /// </summary>
    static Resource()
    {
        _client = CreateClient();
    }

    /// <summary>
    /// The collection that contains the resource.
    /// </summary>
    public static IMongoCollection<T> Collection => GetCollection();

    /// <summary>
    /// Store a <see cref="Resource"/> of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="record">The <see cref="Resource"/> to store.</param>
    /// <exception cref="Conflict">The record violates a unique index.</exception>
    public static T StoreOne(in T record)
    {
        var collection = GetCollection();

        if (record is ISequential sequential && sequential.Sequence == 0)
        {
            sequential.Sequence = Counters.Next(GetDatabase(), _collection!.Name);
        }

        try
        {
            collection.InsertOne(record);
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            throw Conflict(ex);
        }

        return record;
    }

    /// <summary>
    /// Store a <see cref="Resource"/> of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="record">The <see cref="Resource"/> to store.</param>
    /// <exception cref="Conflict">The record violates a unique index.</exception>
    public static async Task<T> StoreOneAsync(T record)
    {
        var collection = GetCollection();

        if (record is ISequential sequential && sequential.Sequence == 0)
        {
            sequential.Sequence = await Counters.NextAsync(GetDatabase(), _collection!.Name);
        }

        try
        {
            await collection.InsertOneAsync(record);
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            throw Conflict(ex);
        }

        return record;
    }

    /// <summary>
    /// Store several resources.
    /// </summary>
    /// <param name="resources">A list of resources.</param>
    /// <exception cref="Conflict">A record violates a unique index.</exception>
    public static void StoreMany(IEnumerable<T> resources)
    {
        var collection = GetCollection();

        foreach (var resource in resources)
        {
            if (resource is ISequential sequential && sequential.Sequence == 0)
            {
                sequential.Sequence = Counters.Next(GetDatabase(), _collection!.Name);
            }
        }

        try
        {
            collection.InsertMany(resources);
        }
        catch (MongoBulkWriteException ex) when (ex.WriteErrors.Any(e => e.Category == ServerErrorCategory.DuplicateKey))
        {
            throw Conflict(ex);
        }
    }

    /// <summary>
    /// Store several resources.
    /// </summary>
    /// <param name="resources">A list of resources.</param>
    /// <exception cref="Conflict">A record violates a unique index.</exception>
    public static async Task StoreManyAsync(IEnumerable<T> resources)
    {
        var collection = GetCollection();

        foreach (var resource in resources)
        {
            if (resource is ISequential sequential && sequential.Sequence == 0)
            {
                sequential.Sequence = await Counters.NextAsync(GetDatabase(), _collection!.Name);
            }
        }

        try
        {
            await collection.InsertManyAsync(resources);
        }
        catch (MongoBulkWriteException ex) when (ex.WriteErrors.Any(e => e.Category == ServerErrorCategory.DuplicateKey))
        {
            throw Conflict(ex);
        }
    }

    /// <summary>
    /// Read a <see cref="Resource"/>.
    /// </summary>
    /// <param name="id">The document's identification number.</param>
    /// <returns>A of type <typeparamref name="T"/>.</returns>
    public static T Read(string id)
    {
        return First(record => record.Id.Equals(id));
    }

    /// <summary>
    /// Read a <see cref="Resource"/>.
    /// </summary>
    /// <param name="id">The document's identification number.</param>
    /// <returns>A of type <typeparamref name="T"/>.</returns>
    public static Task<T> ReadAsync(string id)
    {
        return FirstAsync(record => record.Id.Equals(id));
    }

    /// <summary>
    /// Get whether or not a <see cref="Resource"/> exists.
    /// </summary>
    /// <param name="filter">An optional filter.</param>
    /// <returns>Whether or not the <see cref="Resource"/> exists.</returns>
    public static bool Exists(Expression<Func<T, bool>>? filter = null) => FirstOrDefault(filter) is not null;

    /// <summary>
    /// Get the first matching <see cref="Resource"/>.
    /// </summary>
    /// <param name="filter">An optional filter.</param>
    /// <returns>A <see cref="Resource"/> of type <typeparamref name="T"/>.</returns>
    public static T First(Expression<Func<T, bool>>? filter = null)
    {
        return List(filter).First();
    }

    /// <summary>
    /// Get the first matching <see cref="Resource"/>.
    /// </summary>
    /// <param name="filter">An optional filter.</param>
    /// <returns>A <see cref="Resource"/> of type <typeparamref name="T"/>.</returns>
    public static async Task<T> FirstAsync(Expression<Func<T, bool>>? filter = null)
    {
        return (await ListAsync(filter)).First();
    }

    /// <summary>
    /// Get the first matching <see cref="Resource"/>.
    /// </summary>
    /// <param name="filter">An optional filter.</param>
    /// <returns>A document of type <typeparamref name="T"/>, or null if the <see cref="Resource"/> does not exist.</returns>
    public static T? FirstOrDefault(Expression<Func<T, bool>>? filter = null)
    {
        return List(filter).FirstOrDefault();
    }

    /// <summary>
    /// Get the first matching <see cref="Resource"/>.
    /// </summary>
    /// <param name="filter">An optional filter.</param>
    /// <returns>A document of type <typeparamref name="T"/>, or null if the <see cref="Resource"/> does not exist.</returns>
    public static async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null)
    {
        return (await ListAsync(filter)).FirstOrDefault();
    }

    /// <summary>
    /// List documents of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="filter">A filter for the list.</param>
    /// <returns>A list of documents.</returns>
    public static List<T> List(Expression<Func<T, bool>>? filter = null)
    {
        return GetCollection()
            .Find(filter ?? FilterDefinition<T>.Empty)
            .ToList();
    }

    /// <summary>
    /// List documents of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="filter">A filter for the list.</param>
    /// <returns>A list of documents.</returns>
    public static async Task<List<T>> ListAsync(Expression<Func<T, bool>>? filter = null)
    {
        return (await GetCollection()
            .FindAsync(filter ?? FilterDefinition<T>.Empty))
            .ToList();
    }

    /// <summary>
    /// Replace a <see cref="Resource"/> in the database.
    /// </summary>
    /// <param name="filter">A filter for documents to replace.</param>
    /// <param name="replacement">A replacement <see cref="Resource"/>.</param>
    /// <exception cref="Conflict">The replacement violates a unique index.</exception>
    public static void ReplaceOne(Expression<Func<T, bool>> filter, T replacement)
    {
        try
        {
            GetCollection().ReplaceOne(filter, replacement);
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            throw Conflict(ex);
        }
    }

    /// <summary>
    /// Replace a <see cref="Resource"/> in the database.
    /// </summary>
    /// <param name="filter">A filter for documents to replace.</param>
    /// <param name="replacement">A replacement <see cref="Resource"/>.</param>
    /// <exception cref="Conflict">The replacement violates a unique index.</exception>
    public static async Task ReplaceOneAsync(Expression<Func<T, bool>> filter, T replacement)
    {
        try
        {
            await GetCollection().ReplaceOneAsync(filter, replacement);
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            throw Conflict(ex);
        }
    }

    /// <summary>
    /// Replace multiple resources in the database.
    /// </summary>
    /// <param name="resources">The resources to replace.</param>
    /// <exception cref="Conflict">A replacement violates a unique index.</exception>
    public static void ReplaceMany(IEnumerable<T> resources)
    {
        var models = resources.Select(resource => new ReplaceOneModel<T>(Builders<T>.Filter.Eq(r => r.Id, resource.Id), resource) {
            IsUpsert = true
        });

        try
        {
            GetCollection().BulkWrite(models);
        }
        catch (MongoBulkWriteException ex) when (ex.WriteErrors.Any(e => e.Category == ServerErrorCategory.DuplicateKey))
        {
            throw Conflict(ex);
        }
    }

    /// <summary>
    /// Replace multiple resources in the database.
    /// </summary>
    /// <param name="resources">The resources to replace.</param>
    /// <exception cref="Conflict">A replacement violates a unique index.</exception>
    public static async Task ReplaceManyAsync(IEnumerable<T> resources)
    {
        var models = resources.Select(resource => new ReplaceOneModel<T>(Builders<T>.Filter.Eq(r => r.Id, resource.Id), resource) {
            IsUpsert = true
        });

        try
        {
            await GetCollection().BulkWriteAsync(models);
        }
        catch (MongoBulkWriteException ex) when (ex.WriteErrors.Any(e => e.Category == ServerErrorCategory.DuplicateKey))
        {
            throw Conflict(ex);
        }
    }

    /// <summary>
    /// Remove a <see cref="Resource"/> from the database.
    /// </summary>
    /// <param name="filter">A filter for the removal.</param>
    public static void RemoveOne(Expression<Func<T, bool>>? filter = null)
    {
        GetCollection().DeleteOne(filter ?? FilterDefinition<T>.Empty);
    }

    /// <summary>
    /// Remove a <see cref="Resource"/> from the database.
    /// </summary>
    /// <param name="filter">A filter for the removal.</param>
    public static Task RemoveOneAsync(Expression<Func<T, bool>>? filter = null)
    {
        return GetCollection().DeleteOneAsync(filter ?? FilterDefinition<T>.Empty);
    }

    /// <summary>
    /// Remove documents from the database.
    /// </summary>
    /// <param name="filter">A filter for the removal.</param>
    public static void RemoveMany(Expression<Func<T, bool>>? filter = null)
    {
        GetCollection().DeleteMany(filter ?? FilterDefinition<T>.Empty);
    }

    /// <summary>
    /// Remove documents from the database.
    /// </summary>
    /// <param name="filter">A filter for the removal.</param>
    public static Task RemoveManyAsync(Expression<Func<T, bool>>? filter = null)
    {
        return GetCollection().DeleteManyAsync(filter ?? FilterDefinition<T>.Empty);
    }

    /// <summary>
    /// Count documents of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="filter">An optional filter.</param>
    /// <returns>The number of matching documents.</returns>
    public static long Count(Expression<Func<T, bool>>? filter = null)
    {
        return GetCollection().CountDocuments(filter ?? FilterDefinition<T>.Empty);
    }

    /// <summary>
    /// Count documents of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="filter">An optional filter.</param>
    /// <returns>The number of matching documents.</returns>
    public static Task<long> CountAsync(Expression<Func<T, bool>>? filter = null)
    {
        return GetCollection().CountDocumentsAsync(filter ?? FilterDefinition<T>.Empty);
    }

    private static IMongoDatabase GetDatabase()
    {
        return _client.GetDatabase(Application.Current.Configuration.Database.Name);
    }

    private static IMongoCollection<T> GetCollection()
    {
        _collection ??= typeof(T).GetCustomAttribute<CollectionAttribute>() ?? throw new InvalidOperationException("Specify a collection for this document using the Collection attribute.");

        var database = GetDatabase();

        if (!database.ListCollectionNames().ToList().Contains(_collection.Name))
        {
            CreateCollection(database);
        }

        var collection = database.GetCollection<T>(_collection.Name);

        Index(collection);

        return collection;
    }

    private static void CreateCollection(IMongoDatabase database)
    {
        var options = new CreateCollectionOptions {
            ExpireAfter = _collection!.TTL != Expiration.None ? TimeSpan.FromMilliseconds((long) _collection.TTL) : null
        };

        if (!_collection.TimeSeries)
        {
            database.CreateCollection(_collection.Name, options);
            return;
        }

        options.TimeSeriesOptions = new TimeSeriesOptions(
            timeField: _collection.TimeField,
            metaField: _collection.MetaField,
            granularity: _collection.Granularity switch {
                Granularity.Seconds => TimeSeriesGranularity.Seconds,
                Granularity.Minutes => TimeSeriesGranularity.Minutes,
                Granularity.Hours => TimeSeriesGranularity.Hours,
                _ => TimeSeriesGranularity.Seconds
            });

        database.CreateCollection(_collection.Name, options);

        var collection = database.GetCollection<T>(_collection.Name);
        var keys = Builders<T>.IndexKeys
            .Ascending($"{_collection.MetaField}.{Constants.MetricKey}")
            .Ascending(_collection.TimeField);

        collection.Indexes.CreateOne(new CreateIndexModel<T>(keys));
    }

    private static void Index(IMongoCollection<T> collection)
    {
        if (_indexed)
        {
            return;
        }

        var indexes = typeof(T).GetCustomAttributes<IndexAttribute>();
        var existing = collection.Indexes.List().ToList();

        foreach (var index in indexes)
        {
            var keys = Builders<T>.IndexKeys.Combine(index.Fields.Select(f => Builders<T>.IndexKeys.Ascending(f)));
            var document = keys.Render(new RenderArgs<T>(collection.DocumentSerializer, collection.Settings.SerializerRegistry));
            var conflict = existing.FirstOrDefault(e => e["name"].AsString == index.Name || e["key"] == document);

            if (conflict != default && !Matches(conflict, document, index))
            {
                collection.Indexes.DropOne(conflict["name"].AsString);
                conflict = default;
            }

            if (conflict == default)
            {
                var options = new CreateIndexOptions {
                    Unique = index.Unique,
                    Name = index.Name,
                    Collation = index.CaseInsensitive ? new Collation("en", strength: CollationStrength.Secondary) : null
                };

                collection.Indexes.CreateOne(new CreateIndexModel<T>(keys, options));
            }
        }

        _indexed = true;
    }

    private static bool Matches(BsonDocument existing, BsonDocument keys, IndexAttribute index)
    {
        if (existing["name"].AsString != index.Name || existing["key"] != keys)
        {
            return false;
        }

        if (existing.Contains("unique") && existing["unique"].AsBoolean != index.Unique)
        {
            return false;
        }

        return existing.Contains("collation") == index.CaseInsensitive;
    }

    private static Fault Conflict(Exception inner)
    {
        return new Fault(
            innerException: inner,
            error: new Conflict(
                message: $"A {typeof(T).Name} matching a unique index already exists.", 
                index: inner is MongoWriteException mwe ? Regex.Match(mwe.WriteError.Message, @"index:\s*(\S+)").Groups[1].Value is { Length: > 0 } m ? m : null : null));
    }

    private static MongoClient CreateClient()
    {
        return new MongoClient(Application.Current.Configuration.Database.ConnectionString);
    }
}