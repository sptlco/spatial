// Copyright © Spatial Corporation. All rights reserved.

using MongoDB.Bson;

namespace Spatial;

/// <summary>
/// A unique identifier.
/// </summary>
public class UID
{
    private readonly ObjectId _id;

    /// <summary>
    /// Create a new <see cref="UID"/>.
    /// </summary>
    public UID()
    {
        _id = ObjectId.GenerateNewId();
    }

    /// <summary>
    /// Cast a <see cref="UID"/> to a <see cref="string"/>.
    /// </summary>
    /// <param name="id">A <see cref="UID"/> instance.</param>
    public static implicit operator string(UID id) => id.ToString();

    /// <summary>
    /// Convert the <see cref="UID"/> to a <see cref="string"/>.
    /// </summary>
    /// <returns>A <see cref="string"/> representation of the <see cref="UID"/>.</returns>
    public override string ToString()
    {
        return _id.ToString();
    }
}