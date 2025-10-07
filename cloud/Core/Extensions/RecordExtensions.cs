// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;
using System.Linq.Expressions;

namespace Spatial.Extensions;

/// <summary>
/// Extension methods for <see cref="Record"/>.
/// </summary>
public static class RecordExtensions
{
    /// <summary>
    /// Store a <see cref="Record"/> of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Record"/> to store.</typeparam>
    /// <param name="record">The <see cref="Record"/> to store.</param>
    public static void Store<T>(this T record) where T : Record
    {
        Record<T>.Store(record);
    }

    /// <summary>
    /// Save a <see cref="Record"/> of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Record"/> to save.</typeparam>
    /// <param name="record">The <see cref="Record"/> to save.</param>
    public static void Save<T>(this T record) where T : Record
    {
        Replace(record, r => r.Id == record.Id);
    }

    /// <summary>
    /// Update a <see cref="Record"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Record"/> to save.</typeparam>
    /// <param name="record">The <see cref="Record"/> to update.</param>
    /// <param name="update">An update.</param>
    public static void Update<T>(this T record, Action<T> update) where T : Record
    {
        update(record);
        record.Save();
    }

    /// <summary>
    /// Replace a <see cref="Record"/> of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Record"/> to replace.</typeparam>
    /// <param name="record">A replacement <see cref="Record"/>.</param>
    /// <param name="filter">A filter for the <see cref="Record"/> to replace.</param>
    public static void Replace<T>(this T record, Expression<Func<T, bool>> filter) where T : Record
    {
        Record<T>.Replace(filter, record);
    }

    /// <summary>
    /// Remove a stored <see cref="Record"/> of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Record"/> to delete.</typeparam>
    /// <param name="record">The <see cref="Record"/> to remove.</param>
    public static void Remove<T>(this T record) where T : Record
    {
        Record<T>.RemoveOne(r => r.Id == record.Id);
    }
}
