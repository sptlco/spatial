// Copyright © Spatial. All rights reserved.

using Ignite.Models;
using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// Identifiable metadata for an <see cref="Entity"/> at runtime.
/// </summary>
/// <param name="Handle">The object's unique handle.</param>
public record struct Tag(ushort Handle) : IComponent
{
    /// <summary>
    /// The entity's <see cref="ObjectType"/>.
    /// </summary>
    public readonly ObjectType Type => Decode().Type;

    /// <summary>
    /// The object's identification number.
    /// </summary>
    public readonly ushort Id => Decode().Id;

    /// <summary>
    /// Cast a <see cref="Tag"/> to a <see cref="ushort"/>.
    /// </summary>
    /// <param name="tag">A <see cref="Tag"/>.</param>
    public static implicit operator ushort(Tag tag) => tag.Handle;

    /// <summary>
    /// Cast a <see cref="ushort"/> to a <see cref="Tag"/>.
    /// </summary>
    /// <param name="handle">A <see cref="ushort"/>.</param>
    public static implicit operator Tag(ushort handle) => new(handle);

    /// <summary>
    /// Convert the <see cref="Tag"/> to a <see cref="string"/>.
    /// </summary>
    /// <returns>A <see cref="string"/>.</returns>
    public override readonly string ToString()
    {
        return $"{Type} {Id}";
    }

    private readonly (ObjectType Type, ushort Id) Decode()
    {
        if (Handle >= 8000)
        {
            if (Handle >= 9500)
            {
                if (Handle >= 10500)
                {
                    if (Handle >= 13500)
                    {
                        if (Handle >= 17084)
                        {
                            if (Handle >= 17340)
                            {
                                if (Handle >= 19388)
                                {
                                    if (Handle >= 20388)
                                    {
                                        if (Handle >= 20638)
                                        {
                                            if (Handle >= 21638)
                                            {
                                                if (Handle >= 22138)
                                                {
                                                    if (Handle >= 23138)
                                                    {
                                                        if (Handle < 23638 || Handle >= 25138)
                                                        {
                                                            return ((ObjectType) (-1), ushort.MaxValue);
                                                        }
                                                        else
                                                        {
                                                            return (ObjectType.Pet, (ushort) (Handle - 23638));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        return (ObjectType.Mount, (ushort) (Handle - 22138));
                                                    }
                                                }
                                                else
                                                {
                                                    return (ObjectType.Servant, (ushort) (Handle - 21638));
                                                }
                                            }
                                            else
                                            {
                                                return (ObjectType.Door, (ushort) (Handle - 20638));
                                            }
                                        }
                                        else
                                        {
                                            return (ObjectType.MagicField, (ushort) (Handle - 20388));
                                        }
                                    }
                                    else
                                    {
                                        return (ObjectType.Effect, (ushort) (Handle - 19388));
                                    }
                                }
                                else
                                {
                                    return (ObjectType.Bandit, (ushort) (Handle - 17340));
                                }
                            }
                            else
                            {
                                return (ObjectType.NPC, (ushort) (Handle - 17084));
                            }
                        }
                        else
                        {
                            return (ObjectType.Chunk, (ushort) (Handle - 13500));
                        }
                    }
                    else
                    {
                        return (ObjectType.Drop, (ushort) (Handle - 10500));
                    }
                }
                else
                {
                    return (ObjectType.House, (ushort) (Handle - 9500));
                }
            }
            else
            {
                return (ObjectType.Player, (ushort) (Handle - 8000));
            }
        }
        else
        {
            return (ObjectType.Mob, Handle);
        }
    }
}