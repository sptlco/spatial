// Copyright © Spatial Corporation. All rights reserved.

using StackExchange.Redis;
using System.Diagnostics.CodeAnalysis;
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
    public T Get<T>(string group, string key) => Get<T>(CreateKey(group, key));

    /// <summary>
    /// Get an <see cref="object"/> of type <typeparamref name="T"/> from the <see cref="Cache"/>.
    /// </summary>
    /// <typeparam name="T">The object's <see cref="Type"/>.</typeparam>
    /// <param name="key">The object's key in the <see cref="Cache"/>.</param>
    /// <returns>An <see cref="object"/> of type <typeparamref name="T"/>.</returns>
    public T Get<T>(string key)
    {
        return JsonSerializer.Deserialize<T>(GetValue(key)!)!;
    }

    /// <summary>
    /// Attempt to get a value from the <see cref="Cache"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of value to get.</typeparam>
    /// <param name="group">The value's logical group.</param>
    /// <param name="key">The value's key.</param>
    /// <param name="value">The value.</param>
    /// <returns>Whether or not the value was fetched.</returns>
    public bool TryGet<T>(string group, string key, [MaybeNullWhen(false)] out T? value) => TryGet(CreateKey(group, key), out value);

    /// <summary>
    /// Attempt to get a value from the <see cref="Cache"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of value to get.</typeparam>
    /// <param name="key">The value's key.</param>
    /// <param name="value">The value.</param>
    /// <returns>Whether or not the value was fetched.</returns>
    public bool TryGet<T>(string key, [MaybeNullWhen(false)] out T value)
    {
        value = default;

        var json = GetValue(key);

        if (!json.HasValue)
        {
            return false;
        }

        value = JsonSerializer.Deserialize<T>(json!)!;

        return true;
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
    public T Set<T>(string group, string key, T value, Time? ttl = null) => Set(CreateKey(group, key), value, ttl);

    /// <summary>
    /// Set a <paramref name="key"/> in the <see cref="Cache"/> to a specified <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T">The value's <see cref="Type"/>.</typeparam>
    /// <param name="key">The object's unique key.</param>
    /// <param name="value">The object's value.</param>
    /// <param name="ttl">The object's time-to-live, after which its data is invalidated.</param>
    /// <returns>The <see cref="object"/> added to the <see cref="Cache"/>.</returns>
    public T Set<T>(string key, T value, Time? ttl = null)
    {
        GetDatabase().StringSet(key, JsonSerializer.Serialize(value), ttl?.AsTimeSpan());

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
    public T GetOrSet<T>(string group, string key, Func<string, T> factory, Time? ttl = null) => GetOrSet(CreateKey(group, key), factory, ttl);

    /// <summary>
    /// Get an <see cref="object"/> of type <typeparamref name="T"/> from the <see cref="Cache"/>, or set it 
    /// if it doesn't exist.
    /// </summary>
    /// <typeparam name="T">The object's <see cref="Type"/>.</typeparam>
    /// <param name="key">The object's key in the <see cref="Cache"/>.</param>
    /// <param name="factory">A factory method for creating the object's value.</param>
    /// <param name="ttl">The object's time-to-live, after which its data is invalidated.</param>
    /// <returns>An <see cref="object"/> of type <typeparamref name="T"/>.</returns>
    public T GetOrSet<T>(string key, Func<string, T> factory, Time? ttl = null)
    {
        return TryGet<T>(key, out var value) ? value : Set(key, factory(key), ttl);
    }

    private IDatabase GetDatabase()
    {
        return _cache.GetDatabase(Configuration.Current.Cache.Database);
    }

    private string CreateKey(string group, string key)
    {
        return string.Join(".", group, key);
    }

    private RedisValue GetValue(string key)
    {
        return GetDatabase().StringGet(key);
    }
}