// Copyright © Spatial Corporation. All rights reserved.

using StackExchange.Redis;
using System.Text.Json;

namespace Spatial.Caching;

/// <summary>
/// A thread-safe, Redis-backed cache.
/// </summary>
public class Cache
{
    private readonly ConnectionMultiplexer _cache;

    /// <summary>
    /// Create a new <see cref="Cache"/>.
    /// </summary>
    public Cache()
    {
        _cache = ConnectionMultiplexer.Connect(Configuration.Current.Cache.Url);
    }

    /// <summary>
    /// Get an <see cref="object"/> of type <typeparamref name="T"/> from the <see cref="Cache"/>.
    /// </summary>
    /// <typeparam name="T">The object's <see cref="Type"/>.</typeparam>
    /// <param name="group">The object's logical grouping.</param>
    /// <param name="key">The object's key in the <see cref="Cache"/>.</param>
    /// <returns>An <see cref="object"/> of type <typeparamref name="T"/>.</returns>
    public T? GetOrDefault<T>(string group, string key) => GetOrDefault<T>(CreateKey(group, key));

    /// <summary>
    /// Get an <see cref="object"/> of type <typeparamref name="T"/> from the <see cref="Cache"/>.
    /// </summary>
    /// <typeparam name="T">The object's <see cref="Type"/>.</typeparam>
    /// <param name="key">The object's key in the <see cref="Cache"/>.</param>
    /// <returns>An <see cref="object"/> of type <typeparamref name="T"/>.</returns>
    public T? GetOrDefault<T>(string key)
    {
        var json = (string?) GetDatabase().StringGet(key);

        if (json is null)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(json);
    }

    /// <summary>
    /// Set a <paramref name="key"/> in the <see cref="Cache"/> to a specified <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T">The value's <see cref="Type"/>.</typeparam>
    /// <param name="group">The object's logical group.</param>
    /// <param name="key">The object's unique key.</param>
    /// <param name="value">The object's value.</param>
    /// <param name="ttl">The object's time-to-live, after which its data is invalidated.</param>
    /// <returns>The <see cref="object"/> added to the <see cref="Cache"/>.</returns>
    public T Set<T>(string group, string key, T value, TimeSpan? ttl = default) => Set(CreateKey(group, key), value, ttl);

    /// <summary>
    /// Set a <paramref name="key"/> in the <see cref="Cache"/> to a specified <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T">The value's <see cref="Type"/>.</typeparam>
    /// <param name="key">The object's unique key.</param>
    /// <param name="value">The object's value.</param>
    /// <param name="ttl">The object's time-to-live, after which its data is invalidated.</param>
    /// <returns>The <see cref="object"/> added to the <see cref="Cache"/>.</returns>
    public T Set<T>(string key, T value, TimeSpan? ttl = default)
    {
        GetDatabase().StringSet(key, JsonSerializer.Serialize(value), ttl);

        return value;
    }

    /// <summary>
    /// Get an <see cref="object"/> of type <typeparamref name="T"/> from the <see cref="Cache"/>, or set it 
    /// if it doesn't exist.
    /// </summary>
    /// <typeparam name="T">The object's <see cref="Type"/>.</typeparam>
    /// <param name="group">The object's logical group.</param>
    /// <param name="key">The object's key in the <see cref="Cache"/>.</param>
    /// <param name="factory">A factory method for creating the object's value.</param>
    /// <param name="ttl">The object's time-to-live, after which its data is invalidated.</param>
    /// <returns>An <see cref="object"/> of type <typeparamref name="T"/>.</returns>
    public T GetOrSet<T>(string group, string key, Func<string, T> factory, TimeSpan? ttl = default) => GetOrSet(CreateKey(group, key), factory, ttl);

    /// <summary>
    /// Get an <see cref="object"/> of type <typeparamref name="T"/> from the <see cref="Cache"/>, or set it 
    /// if it doesn't exist.
    /// </summary>
    /// <typeparam name="T">The object's <see cref="Type"/>.</typeparam>
    /// <param name="key">The object's key in the <see cref="Cache"/>.</param>
    /// <param name="factory">A factory method for creating the object's value.</param>
    /// <param name="ttl">The object's time-to-live, after which its data is invalidated.</param>
    /// <returns>An <see cref="object"/> of type <typeparamref name="T"/>.</returns>
    public T GetOrSet<T>(string key, Func<string, T> factory, TimeSpan? ttl = default)
    {
        return GetOrDefault<T>(key) ?? Set(key, factory(key), ttl);
    }

    private IDatabase GetDatabase(int database = -1)
    {
        return _cache.GetDatabase(database);
    }

    private string CreateKey(string group, string key)
    {
        return string.Join(".", group, key);
    }
}