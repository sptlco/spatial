// Copyright © Spatial. All rights reserved.

namespace Ignite.Models;

/// <summary>
/// The type of inventory an <see cref="Item"/> is stored in.
/// </summary>
public enum InventoryType
{
    /// <summary>
    /// An inventory for guild academy rewards.
    /// </summary>
    Academy = 0,

    /// <summary>
    /// An inventory containing rewarded items.
    /// </summary>
    Rewards = 2,

    /// <summary>
    /// An inventory containing pieces of furniture.
    /// </summary>
    Furniture = 3,

    /// <summary>
    /// An inventory containing items shared across a guild.
    /// </summary>
    Guild = 4,

    /// <summary>
    /// An inventory containing items shared at an account level.
    /// </summary>
    Account = 6,

    /// <summary>
    /// An inventory containing equipped items.
    /// </summary>
    Equipment = 8,

    /// <summary>
    /// An inventory containing items collected by a character.
    /// </summary>
    Character = 9,

    /// <summary>
    /// An inventory containing houses.
    /// </summary>
    House = 12,

    /// <summary>
    /// An inventory containing player actions.
    /// </summary>
    Actions = 15,
}