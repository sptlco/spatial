// Copyright © Spatial. All rights reserved.

using Ignite.Components;
using Spatial.Simulation;

namespace Ignite.Models.Objects;

/// <summary>
/// A reference to a chunk <see cref="Object"/>.
/// </summary>
public class ChunkRef : ObjectRef
{
    /// <summary>
    /// Create a new <see cref="ChunkRef"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> the <see cref="Chunk"/> is in.</param>
    /// <param name="entity">The <see cref="Entity"/> to reference.</param>
    public ChunkRef(Map map, Entity entity) : base(map, entity) { }

    /// <summary>
    /// The referenced <see cref="Chunk"/>.
    /// </summary>
    public Chunk Object => Get<Chunk>();

    /// <summary>
    /// Whether or not the <see cref="Chunk"/> is disabled.
    /// </summary>
    public bool Disabled => Has<Disabled>();

    /// <summary>
    /// Convert the <see cref="ChunkRef"/> to a <see cref="string"/>.
    /// </summary>
    /// <returns>A <see cref="string"/>.</returns>
    public override string ToString()
    {
        return Object.ToString();
    }
}
