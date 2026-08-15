// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Extensions;

/// <summary>
/// Extension methods for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Convert the <see cref="IEnumerable{T}"/> to an array.
    /// </summary>
    /// <typeparam name="TF">The type to convert elements from.</typeparam>
    /// <typeparam name="TR">The type to convert elements to.</typeparam>
    /// <param name="enumerable">An <see cref="IEnumerable{T}"/>.</param>
    /// <param name="mapper">A mapping function.</param>
    /// <returns>An array of type <typeparamref name="TR"/>.</returns>
    public static TR[] ToArray<TF, TR>(this IEnumerable<TF> enumerable, Func<TF, TR> mapper)
    {
        return [.. enumerable.Select(mapper)];
    }

    /// <summary>
    /// Filter the <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to filter.</param>
    /// <param name="predicate">A search predicate.</param>
    /// <returns>The filtered collection.</returns>
    public static IEnumerable<T> Filter<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
        foreach (var value in enumerable)
        {
            if (predicate(value))
            {
                yield return value;
            }
        }
    }

    /// <summary>
    /// Get the first <typeparamref name="T"/> matching <paramref name="predicate"/>.
    /// </summary>
    /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to search.</param>
    /// <param name="predicate">A search predicate.</param>
    /// <returns>The first <typeparamref name="T"/> matching <paramref name="predicate"/>.</returns>
    public static T? Find<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
        foreach (var value in enumerable)
        {
            if (predicate(value))
            {
                return value;
            }
        }

        return default;
    }
}
