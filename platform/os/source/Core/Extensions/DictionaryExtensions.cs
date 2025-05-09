// Copyright © Spatial. All rights reserved.

namespace Spatial.Extensions;

/// <summary>
/// Extension methods for <see cref="Dictionary{TKey, TValue}"/>.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Get a value, or add it if the key doesn't exist.
    /// </summary>
    /// <param name="dictionary">A dictionary.</param>
    /// <param name="key">A key value.</param>
    /// <param name="factory">A factory method used to create a value.</param>
    /// <typeparam name="TKey">The dictionary's key type.</typeparam>
    /// <typeparam name="TValue">The dictionary's value type.</typeparam>
    /// <returns>The value at the specified key.</returns>
    public static TValue GetOrAdd<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, TValue> factory)
    {
        if (!dictionary.TryGetValue(key, out var value))
        {
            value = factory(key);
            dictionary.Add(key, value);
        }

        return value;
    }

    /// <summary>
    /// Process all elements in the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The dictionary's key type.</typeparam>
    /// <typeparam name="TValue">The dictionary's value type.</typeparam>
    /// <param name="dictionary">The dictionary to process.</param>
    /// <param name="function">An <see cref="Action"/> to process.</param>
    public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Action<TKey, TValue> function)
    {
        foreach (var (key, value) in dictionary)
        {
            function(key, value);
        }
    }
}
