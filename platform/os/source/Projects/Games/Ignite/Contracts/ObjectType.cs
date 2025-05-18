// Copyright © Spatial. All rights reserved.

using Ignite.Models.Objects;

namespace Ignite.Contracts;

/// <summary>
/// Specifies the type of an <see cref="ObjectRef"/>.
/// </summary>
public enum ObjectType
{
    Chunk = 0,
    Drop = 1,
    Player = 2,
    House = 3,
    NPC = 4,
    Mob = 5,
    MagicField = 6,
    Door = 7,
    Bandit = 8,
    Effect = 9,
    Servant = 10,
    Mount = 11,
    Pet = 12
}