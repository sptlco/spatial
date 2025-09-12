// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Extensions;

/// <summary>
/// Extension methods for a collection.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Convert a collection to a padded array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the original array.</typeparam>
    /// <typeparam name="TR">The type of elements in the padded array.</typeparam>
    /// <param name="collection">The collection to convert.</param>
    /// <param name="size">The size of the padded array.</param>
    /// <param name="selector">A selector function.</param>
    /// <param name="value">A padding value.</param>
    /// <returns>A padded array.</returns>
    public static TR[] ToPaddedArray<T, TR>(this ICollection<T> collection, int size, Func<T, TR> selector, TR value)
    {
        var destination = new TR[size];

        for (var i = 0; i < collection.Count; i++)
        {
            destination[i] = selector(collection.ElementAt(i));
        }

        for (var i = 0; i < destination.Length - collection.Count; i++)
        {
            destination[collection.Count + i] = value;
        }

        return destination;
    }

    /// <summary>
    /// Execute an action for each element of a collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">A collection of elements.</param>
    /// <param name="action">An action to execute.</param>
    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        foreach (var element in collection)
        {
            action(element);
        }
    }
}
