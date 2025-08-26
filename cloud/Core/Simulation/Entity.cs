// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Simulation;

/// <summary>
/// A unique object in the simulation.
/// </summary>
/// <param name="Handle">An encoded identifier for the <see cref="Entity"/>.</param>
public record struct Entity(UInt128 Handle)
{
    /// <summary>
    /// A null entity.
    /// </summary>
    public static readonly Entity Null = new(0);

    /// <summary>
    /// Create a new <see cref="Entity"/>.
    /// </summary>
    /// <param name="id">A unique identification number.</param>
    public Entity(uint id) : this((UInt128) id << 96)
    {
    }

    /// <summary>
    /// The entity's unique identification number.
    /// </summary>
    public readonly uint Id => (uint) (Handle >> 96);

    /// <summary>
    /// The entity's <see cref="Archetype"/>.
    /// </summary>
    internal uint Archetype
    {
        readonly get => (uint) (Handle >> 64);
        set => Handle = (Handle & ~((UInt128) 0xFFFFFFFF << 64)) | ((UInt128) value << 64);
    }

    /// <summary>
    /// The entity's <see cref="Chunk"/>.
    /// </summary>
    internal uint Chunk
    {
        readonly get => (uint) (Handle >> 32);
        set => Handle = (Handle & ~((UInt128) 0xFFFFFFFF << 32)) | ((UInt128) value << 32);
    }

    /// <summary>
    /// The entity's index in the <see cref="Chunk"/>.
    /// </summary>
    internal uint Index
    {
        readonly get => (uint) Handle;
        set => Handle = (Handle & ~(UInt128) 0xFFFFFFFF) | value;
    }

    /// <summary>
    /// Cast the <see cref="Entity"/> to a <see cref="uint"/>.
    /// </summary>
    /// <param name="entity">An <see cref="Entity"/>.</param>
    /// <returns>The entity's identification number.</returns>
    public static implicit operator uint(Entity entity) => entity.Id;

    /// <summary>
    /// Cast the <see cref="uint"/> to an <see cref="Entity"/>.
    /// </summary>
    /// <param name="entity">An <see cref="Entity"/> identification number.</param>
    /// <returns>An <see cref="Entity"/>.</returns>
    public static implicit operator Entity(uint entity) => new(entity);

    /// <summary>
    /// Get whether or not the <see cref="Entity"/> equals another <see cref="Entity"/>.
    /// </summary>
    /// <param name="other">Another <see cref="Entity"/>.</param>
    /// <returns>Whether or not the two entities are equal.</returns>
    public readonly bool Equals(Entity other) => Id == other.Id;

    /// <summary>
    /// Get the hash code of the <see cref="Entity"/>.
    /// </summary>
    /// <returns>The entity's hash code.</returns>
    public override readonly int GetHashCode() => Id.GetHashCode();
}