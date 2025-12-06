// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Cloud.Models;

/// <summary>
/// ...
/// </summary>
public class Resource : Record
{
    /// <summary>
    /// Create a new <see cref="Resource"/>.
    /// </summary>
    /// <param name="name">The name of the <see cref="Resource"/>.</param>
    public Resource(string? name = default)
    {
        Name = name ?? GetType().Name;
        Seed = Strong.Int32();
    }

    /// <summary>
    /// The name of the <see cref="Resource"/>.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// A random number generated for secure access to the <see cref="Resource"/>.
    /// </summary>
    public int Seed { get; set; }

    /// <summary>
    /// The <see cref="Resource"/> that owns this <see cref="Resource"/>.
    /// </summary>
    public string Owner { get; set; }

    /// <summary>
    /// ...
    /// </summary>
    public Dictionary<string, string> Properties { get; set; } = [];

    /// <summary>
    /// Query the <see cref="Resource"/> for a property.
    /// </summary>
    /// <typeparam name="T">The property's return type.</typeparam>
    /// <param name="key">The property to query.</param>
    /// <returns>The property's value.</returns>
    public T Query<T>(string key) => (T) Convert.ChangeType(Properties[key], typeof(T));
}