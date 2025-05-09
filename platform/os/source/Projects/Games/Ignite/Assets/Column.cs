// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets;

/// <summary>
/// Contains specifications for column data.
/// </summary>
public class Column
{
    /// <summary>
    /// Create a new <see cref="Column"/>.
    /// </summary>
    /// <param name="name">The name of the <see cref="Column"/>.</param>
    public Column(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Create a new <see cref="Column"/>.
    /// </summary>
    /// <param name="type">The column's data type.</param>
    public Column(DataType type) : this(string.Empty, type)
    {
    }

    /// <summary>
    /// Create a new <see cref="Column"/>.
    /// </summary>
    /// <param name="name">The name of the <see cref="Column"/>.</param>
    /// <param name="type">The column's data type.</param>
    /// <param name="size">The size of the data in the <see cref="Column"/>.</param>
    public Column(string name, DataType type, uint size = 0)
    {
        Name = name;
        Type = type;
        Size = size;
    }

    /// <summary>
    /// The name of the <see cref="Column"/>.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The column's data type.
    /// </summary>
    public DataType Type { get; set; }

    /// <summary>
    /// The size of the data in the <see cref="Column"/>.
    /// </summary>
    public uint Size { get; set; }

    /// <summary>
    /// The number of columns this definition specifies.
    /// </summary>
    public uint Count { get; set; } = 1;
}