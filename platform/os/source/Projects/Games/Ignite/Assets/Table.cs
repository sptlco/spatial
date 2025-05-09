// Copyright © Spatial. All rights reserved.

using System.Collections.Generic;

namespace Ignite.Assets;

/// <summary>
/// A tabular <see cref="Asset"/>.
/// </summary>
public class Table : Asset
{
    /// <summary>
    /// Create a new <see cref="Table"/>.
    /// </summary>
    public Table()
    {
        Records = [];
        Children = [];
    }

    /// <summary>
    /// The records in the <see cref="Table"/>.
    /// </summary>
    public List<object> Records { get; set; }

    /// <summary>
    /// The name of the table's parent resource.
    /// </summary>
    public string? Parent { get; set; }

    /// <summary>
    /// Paths to this table's child resources.
    /// </summary>
    public List<Table> Children { get; set; }
}