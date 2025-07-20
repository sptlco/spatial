// Copyright © Spatial Corporation. All rights reserved.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Reflection;

namespace Spatial.Persistence;

/// <summary>
/// A record stored in the database.
/// </summary>
public class Document
{
    /// <summary>
    /// The document's identification number.
    /// </summary>
    [BsonId]
    public uint Id { get; set; }

    /// <summary>
    /// The <see cref="DateTime"/> the <see cref="Document"/> was created.
    /// </summary>
    public DateTime Created { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// A record stored in the database.
/// </summary>
public static class Document<T> where T : Document
{
    private static MongoClient _client;
    private static string? _collection;

    /// <summary>
    /// Create a new <see cref="Document{T}"/>.
    /// </summary>
    static Document()
    {
        _client = CreateClient();
    }

    /// <summary>
    /// Store a document of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="document">The document to store.</param>
    public static void Store(in T document)
    {
        document.Id = GenerateId();

        GetCollection().InsertOne(document);
    }

    /// <summary>
    /// Read a document.
    /// </summary>
    /// <param name="id">The document's identification number.</param>
    /// <returns>A document of type <typeparamref name="T"/>.</returns>
    public static T Read(uint id)
    {
        return First(doc => doc.Id == id);
    }

    /// <summary>
    /// Get the first matching document.
    /// </summary>
    /// <param name="filter">An optional filter.</param>
    /// <returns>A document of type <typeparamref name="T"/>.</returns>
    public static T First(Expression<Func<T, bool>>? filter = null)
    {
        return List(filter).First();
    }

    /// <summary>
    /// Get the first matching document.
    /// </summary>
    /// <param name="filter">An optional filter.</param>
    /// <returns>A document of type <typeparamref name="T"/>, or null if the document does not exist.</returns>
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
    /// Replace a document in the database.
    /// </summary>
    /// <param name="filter">A filter for documents to replace.</param>
    /// <param name="replacement">A replacement document.</param>
    public static void Replace(Expression<Func<T, bool>> filter, T replacement)
    {
        GetCollection().ReplaceOne(filter, replacement);
    }

    /// <summary>
    /// Remove a document from the database.
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

    private static uint GenerateId()
    {
        var filter = Builders<Counter>.Filter.Eq(c => c.Name, typeof(T).Name);
        var update = Builders<Counter>.Update.Inc<uint>(c => c.Count, 1);

        var options = new FindOneAndUpdateOptions<Counter> {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        var counter = GetDatabase()
            .GetCollection<Counter>(Constants.CounterCollectionName)
            .FindOneAndUpdate(filter, update, options);

        return counter.Count;
    }

    private static IMongoDatabase GetDatabase()
    {
        return _client.GetDatabase(Application.Current.Configuration.Database.Name);
    }

    private static IMongoCollection<T> GetCollection()
    {
        return GetDatabase().GetCollection<T>(_collection ??= GetCollectionName());
    }

    private static string GetCollectionName()
    {
        var collection = typeof(T).GetCustomAttribute<CollectionAttribute>()?.Name;

        if (string.IsNullOrEmpty(collection))
        {
            throw new InvalidOperationException("Specify a collection for this document using the Collection attribute.");
        }

        return collection;
    }

    private static MongoClient CreateClient()
    {
        return new MongoClient(Application.Current.Configuration.Database.ConnectionString);
    }
}

/// <summary>
/// Counts documents in the database.
/// </summary>
public class Counter
{
    /// <summary>
    /// The counter's identifier.
    /// </summary>
    public ObjectId Id { get; set; }

    /// <summary>
    /// The name of the <see cref="Counter"/>.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The current <see cref="Document"/> count.
    /// </summary>
    public uint Count { get; set; }
}
