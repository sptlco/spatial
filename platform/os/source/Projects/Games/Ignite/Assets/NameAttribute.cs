// Copyright © Spatial. All rights reserved.

using System;

namespace Ignite.Assets;

/// <summary>
/// Specifies the name of a <see cref="Asset"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class NameAttribute : Attribute
{
    /// <summary>
    /// Create a new <see cref="NameAttribute"/>.
    /// </summary>
    /// <param name="name">The name of the <see cref="Asset"/> modeled by the type.</param>
    public NameAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// The name of the <see cref="Asset"/> modeled by the type.
    /// </summary>
    public string Name { get; set; }
}