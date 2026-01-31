// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Scopes;

/// <summary>
/// A brief description of a scope.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class MetadataAttribute : Attribute
{
    /// <summary>
    /// Create a new <see cref="MetadataAttribute"/>.
    /// </summary>
    /// <param name="description">The scope's description.</param>
    public MetadataAttribute(string? description = default)
    {
        Description = description;
    }

    /// <summary>
    /// Create a new <see cref="MetadataAttribute"/>.
    /// </summary>
    /// <param name="icon">The scope's icon.</param>
    /// <param name="description">The scope's value.</param>
    public MetadataAttribute(string icon, string description)
    {
        Icon = icon;
        Description = description;
    }

    /// <summary>
    /// An icon symbolizing the scope.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// A description of the scope.
    /// </summary>
    public string? Description { get; set; }
}