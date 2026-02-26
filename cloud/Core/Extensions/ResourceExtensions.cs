// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;
using System.Linq.Expressions;

namespace Spatial.Extensions;

/// <summary>
/// Extension methods for <see cref="Resource"/>.
/// </summary>
public static class ResourceExtensions
{
    /// <summary>
    /// Store a <see cref="Resource"/> of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Resource"/> to store.</typeparam>
    /// <param name="record">The <see cref="Resource"/> to store.</param>
    public static T Store<T>(this T record) where T : Resource
    {
        Resource<T>.StoreOne(record);

        return record;
    }

    /// <summary>
    /// Save a <see cref="Resource"/> of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Resource"/> to save.</typeparam>
    /// <param name="record">The <see cref="Resource"/> to save.</param>
    public static T Save<T>(this T record) where T : Resource
    {
        Replace(record, r => r.Id == record.Id);

        return record;
    }

    /// <summary>
    /// Update a <see cref="Resource"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Resource"/> to save.</typeparam>
    /// <param name="record">The <see cref="Resource"/> to update.</param>
    /// <param name="update">An update.</param>
    public static T Update<T>(this T record, Action<T> update) where T : Resource
    {
        update(record);

        return record.Save();
    }

    /// <summary>
    /// Replace a <see cref="Resource"/> of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Resource"/> to replace.</typeparam>
    /// <param name="record">A replacement <see cref="Resource"/>.</param>
    /// <param name="filter">A filter for the <see cref="Resource"/> to replace.</param>
    public static void Replace<T>(this T record, Expression<Func<T, bool>> filter) where T : Resource
    {
        Resource<T>.Replace(filter, record);
    }

    /// <summary>
    /// Remove a stored <see cref="Resource"/> of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Resource"/> to delete.</typeparam>
    /// <param name="record">The <see cref="Resource"/> to remove.</param>
    public static void Remove<T>(this T record) where T : Resource
    {
        Resource<T>.RemoveOne(r => r.Id == record.Id);
    }
}
