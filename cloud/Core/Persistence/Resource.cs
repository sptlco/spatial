// Copyright © Spatial Corporation. All rights reserved.

using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Reflection;

namespace Spatial.Persistence;

/// <summary>
/// A document stored in the database.
/// </summary>
public class Resource
{
    /// <summary>
    /// Create a new <see cref="Resource"/>.
    /// </summary>
    public Resource()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    /// <summary>
    /// The document's identification number.
    /// </summary>
    public string Id { get; set; }

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
    public static T Store(in T record)
    {
        GetCollection().InsertOne(record);

        return record;
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
    /// <returns>A document of type <typeparamref name="T"/>, or null if the <see cref="Resource"/> does not exist.</returns>
    public static T? FirstOrDefault(Expression<Func<T, bool>>? filter = null)
    {
        return List(filter).FirstOrDefault();
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
    /// Replace a <see cref="Resource"/> in the database.
    /// </summary>
    /// <param name="filter">A filter for documents to replace.</param>
    /// <param name="replacement">A replacement <see cref="Resource"/>.</param>
    public static void Replace(Expression<Func<T, bool>> filter, T replacement)
    {
        GetCollection().ReplaceOne(filter, replacement);
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
    /// Remove documents from the database.
    /// </summary>
    /// <param name="filter">A filter for the removal.</param>
    public static void RemoveMany(Expression<Func<T, bool>>? filter = null)
    {
        GetCollection().DeleteMany(filter ?? FilterDefinition<T>.Empty);
    }

    private static IMongoDatabase GetDatabase()
    {
        return _client.GetDatabase(Application.Current.Configuration.Database.Name);
    }

    private static IMongoCollection<T> GetCollection()
    {
        _collection ??= typeof(T).GetCustomAttribute<CollectionAttribute>() ?? throw new InvalidOperationException("Specify a collection for this document using the Collection attribute.");
        
        var database = GetDatabase();

        if (!database.ListCollectionNames()
            .ToList()
            .Contains(_collection.Name))
        {
            var options = new CreateCollectionOptions {
                ExpireAfter = _collection.TTL != Expiration.None ? TimeSpan.FromMilliseconds((long) _collection.TTL) : null
            };

            if (_collection.TimeSeries)
            {
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
                    .Ascending($"{_collection.MetaField}.name")
                    .Ascending(_collection.TimeField);

                collection.Indexes.CreateOne(new CreateIndexModel<T>(keys));
            }
            else
            {
                database.CreateCollection(_collection.Name);
            }
        }

        return database.GetCollection<T>(_collection.Name);
    }

    private static MongoClient CreateClient()
    {
        return new MongoClient(Application.Current.Configuration.Database.ConnectionString);
    }
}