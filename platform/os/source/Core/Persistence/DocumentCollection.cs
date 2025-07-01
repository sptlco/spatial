// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Persistence;

/// <summary>
/// Specifies the collection of a <see cref="Document"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DocumentCollection : Attribute
{
    /// <summary>
    /// Create a new <see cref="DocumentCollection"/>.
    /// </summary>
    /// <param name="collection">The name of the collection.</param>
    public DocumentCollection(string collection)
    {
        Collection = collection;
    }

    /// <summary>
    /// The name of the collection.
    /// </summary>
    public string Collection { get; set; }
}
