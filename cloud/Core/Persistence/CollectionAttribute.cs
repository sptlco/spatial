// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Persistence;

/// <summary>
/// Specifies the collection of a <see cref="Record"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class CollectionAttribute : Attribute
{
    /// <summary>
    /// Create a new <see cref="CollectionAttribute"/>.
    /// </summary>
    /// <param name="name">The name of the collection.</param>
    public CollectionAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// The name of the collection.
    /// </summary>
    public string Name { get; set; }
}
