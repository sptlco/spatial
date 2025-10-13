// Copyright © Spatial Corporation. All rights reserved.

using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Reflection;

namespace Spatial.Persistence;

/// <summary>
/// A document stored in the database.
/// </summary>
public class Record
{
    /// <summary>
    /// Create a new <see cref="Record"/>.
    /// </summary>
    public Record()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    /// <summary>
    /// The document's identification number.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The <see cref="DateTime"/> the <see cref="Record"/> was created.
    /// </summary>
    public double Created { get; set; } = Time.Now;
}

/// <summary>
/// A document stored in the database.
/// </summary>
public static class Record<T> where T : Record
{
    private static MongoClient _client;
    private static string? _collection;

    /// <summary>
    /// Create a new <see cref="Record{T}"/>.
    /// </summary>
    static Record()
    {
        _client = CreateClient();
    }

    /// <summary>
    /// Store a <see cref="Record"/> of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="record">The <see cref="Record"/> to store.</param>
    public static void Store(in T record)
    {
        GetCollection().InsertOne(record);
    }

    /// <summary>
    /// Read a <see cref="Record"/>.
    /// </summary>
    /// <param name="id">The document's identification number.</param>
    /// <returns>A of type <typeparamref name="T"/>.</returns>
    public static T Read(string id)
    {
        return First(record => record.Id.Equals(id));
    }

    /// <summary>
    /// Get the first matching <see cref="Record"/>.
    /// </summary>
    /// <param name="filter">An optional filter.</param>
    /// <returns>A <see cref="Record"/> of type <typeparamref name="T"/>.</returns>
    public static T First(Expression<Func<T, bool>>? filter = null)
    {
        return List(filter).First();
    }

    /// <summary>
    /// Get the first matching <see cref="Record"/>.
    /// </summary>
    /// <param name="filter">An optional filter.</param>
    /// <returns>A document of type <typeparamref name="T"/>, or null if the <see cref="Record"/> does not exist.</returns>
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
    /// Replace a <see cref="Record"/> in the database.
    /// </summary>
    /// <param name="filter">A filter for documents to replace.</param>
    /// <param name="replacement">A replacement <see cref="Record"/>.</param>
    public static void Replace(Expression<Func<T, bool>> filter, T replacement)
    {
        GetCollection().ReplaceOne(filter, replacement);
    }

    /// <summary>
    /// Remove a <see cref="Record"/> from the database.
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