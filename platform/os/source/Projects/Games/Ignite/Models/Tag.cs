// Copyright © Spatial. All rights reserved.

using Ignite.Components;
using Ignite.Contracts;

namespace Ignite.Models;

/// <summary>
/// Identifiable metadata for a physical <see cref="Body"/>.
/// </summary>
/// <param name="Handle">The body's unique identification number.</param>
public record struct Tag(ushort Handle)
{
    /// <summary>
    /// Create a new <see cref="Tag"/>.
    /// </summary>
    /// <param name="handle">An identification number.</param>
    public static implicit operator Tag(ushort handle) => new(handle);

    /// <summary>
    /// Create a new <see cref="ushort"/>.
    /// </summary>
    /// <param name="tag">A <see cref="Tag"/>.</param>
    public static implicit operator ushort(Tag tag) => tag.Handle;

    /// <summary>
    /// Decode a <see cref="Tag"/>.
    /// </summary>
    /// <returns>Information about a <see cref="Body"/>.</returns>
    public readonly (BodyType Type, ushort Id) Decode()
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
                                                            return ((BodyType) (-1), ushort.MaxValue);
                                                        }
                                                        else
                                                        {
                                                            return (BodyType.Pet, (ushort) (Handle - 23638));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        return (BodyType.Mount, (ushort) (Handle - 22138));
                                                    }
                                                }
                                                else
                                                {
                                                    return (BodyType.Servant, (ushort) (Handle - 21638));
                                                }
                                            }
                                            else
                                            {
                                                return (BodyType.Door, (ushort) (Handle - 20638));
                                            }
                                        }
                                        else
                                        {
                                            return (BodyType.MagicField, (ushort) (Handle - 20388));
                                        }
                                    }
                                    else
                                    {
                                        return (BodyType.Effect, (ushort) (Handle - 19388));
                                    }
                                }
                                else
                                {
                                    return (BodyType.Bandit, (ushort) (Handle - 17340));
                                }
                            }
                            else
                            {
                                return (BodyType.NPC, (ushort) (Handle - 17084));
                            }
                        }
                        else
                        {
                            return (BodyType.Chunk, (ushort) (Handle - 13500));
                        }
                    }
                    else
                    {
                        return (BodyType.Drop, (ushort) (Handle - 10500));
                    }
                }
                else
                {
                    return (BodyType.House, (ushort) (Handle - 9500));
                }
            }
            else
            {
                return (BodyType.Player, (ushort) (Handle - 8000));
            }
        }
        else
        {
            return (BodyType.Mob, Handle);
        }
    }
}