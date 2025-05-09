// Copyright © Spatial. All rights reserved.

using Spatial.Persistence;
using System.Linq.Expressions;

namespace Spatial.Extensions;

/// <summary>
/// Extension methods for documents.
/// </summary>
public static class DocumentExtensions
{
    /// <summary>
    /// Store a document of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of document to store.</typeparam>
    /// <param name="document">The document to store.</param>
    public static void Store<T>(this T document) where T : Document
    {
        Document<T>.Store(document);
    }

    /// <summary>
    /// Save a document of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of document to save.</typeparam>
    /// <param name="document">The document to save.</param>
    public static void Save<T>(this T document) where T : Document
    {
        Replace(document, d => d.Id == document.Id);
    }

    /// <summary>
    /// Update a <see cref="Document"/>.
    /// </summary>
    /// <typeparam name="T">The type of document to save.</typeparam>
    /// <param name="document">The document to update.</param>
    /// <param name="update">An update.</param>
    public static void Update<T>(this T document, Action<T> update) where T : Document
    {
        update(document);
        document.Save();
    }

    /// <summary>
    /// Replace a document of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of document to replace.</typeparam>
    /// <param name="document">A replacement document.</param>
    /// <param name="filter">A filter for the document to replace.</param>
    public static void Replace<T>(this T document, Expression<Func<T, bool>> filter) where T : Document
    {
        Document<T>.Replace(filter, document);
    }

    /// <summary>
    /// Remove a stored document of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of document to delete.</typeparam>
    /// <param name="document">The document to remove.</param>
    public static void Remove<T>(this T document) where T : Document
    {
        Document<T>.RemoveOne(d => d.Id == document.Id);
    }
}
