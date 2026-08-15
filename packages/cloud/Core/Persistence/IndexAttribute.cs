// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Persistence;

/// <summary>
/// Declares an index for a <see cref="Resource"/> collection.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class IndexAttribute : Attribute
{
    /// <summary>
    /// Create a new <see cref="IndexAttribute"/>.
    /// </summary>
    /// <param name="fields">The fields to index, in order.</param>
    public IndexAttribute(params string[] fields)
    {
        Fields = fields;
    }

    /// <summary>
    /// The fields to index, in order.
    /// </summary>
    public string[] Fields { get; }

    /// <summary>
    /// Whether the index enforces uniqueness.
    /// </summary>
    public bool Unique { get; init; }

    /// <summary>
    /// An explicit name for the index. If specified, this name is used to identify which
    /// index a write violated, rather than relying on MongoDB's auto-generated field-based name.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Whether string comparisons for this index ignore case..
    /// </summary>
    /// <remarks>
    /// Queries and other indexes over the same fields must specify a matching collation to
    /// benefit from this index; MongoDB falls back to a collection scan otherwise.
    /// </remarks>
    public bool CaseInsensitive { get; init; }
}