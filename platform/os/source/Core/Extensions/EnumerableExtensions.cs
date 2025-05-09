// Copyright © Spatial. All rights reserved.

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
}
