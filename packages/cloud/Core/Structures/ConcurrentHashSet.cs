// Copyright © Spatial Corporation. All rights reserved.

using System.Collections;
using System.Collections.Concurrent;

namespace Spatial.Structures;

/// <summary>
/// A thread-safe hash set.
/// </summary>
public class ConcurrentHashSet<T> : IEnumerable<T> where T : notnull
{
    private readonly ConcurrentDictionary<T, byte> _values;

    /// <summary>
    /// Create a new <see cref="ConcurrentHashSet{T}"/>.
    /// </summary>
    public ConcurrentHashSet()
    {
        _values = [];
    }

    /// <summary>
    /// Create a new <see cref="ConcurrentHashSet{T}"/>.
    /// </summary>
    /// <param name="collection">A collection of items.</param>
    public ConcurrentHashSet(IEnumerable<T> collection)
    {
        _values = new ConcurrentDictionary<T, byte>(collection.Select(t => new KeyValuePair<T, byte>(t, 0)));
    }

    /// <summary>
    /// The number of values in the <see cref="ConcurrentHashSet{T}"/>.
    /// </summary>
    public int Count => _values.Count;

    /// <summary>
    /// Add a value to the <see cref="ConcurrentHashSet{T}"/>.
    /// </summary>
    /// <param name="value">The value to add.</param>
    public void Add(T value)
    {
        _values[value] = 0;
    }

    /// <summary>
    /// Remove a value from the <see cref="ConcurrentHashSet{T}"/>.
    /// </summary>
    /// <param name="value">The value to remove.</param>
    public void Remove(T value)
    {
        _values.TryRemove(value, out _);
    }

    /// <summary>
    /// Get whether or not the <see cref="ConcurrentHashSet{T}"/> contains a value.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <returns>Whether or not the <see cref="ConcurrentHashSet{T}"/> contains the value.</returns>
    public bool Contains(T value)
    {
        return _values.ContainsKey(value);
    }

    /// <summary>
    /// Clear the <see cref="ConcurrentHashSet{T}"/>.
    /// </summary>
    public void Clear()
    {
        _values.Clear();
    }

    /// <summary>
    /// Enumerate the <see cref="ConcurrentHashSet{T}"/>.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/>.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        foreach (var kvp in _values)
        {
            yield return kvp.Key;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
