// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// A non-player character inhabiting the <see cref="Models.World"/>.
/// </summary>
/// <param name="Id">The NPC's identification number.</param>
/// <param name="Menu">Whether or not the <see cref="NPC"/> displays a menu.</param>
/// <param name="Role">The NPC's <see cref="NPCRole"/>.</param>
/// <param name="Type">The NPC's <see cref="NPCType"/>.</param>
public record struct NPC(
    ushort Id,
    bool Menu,
    NPCRole Role,
    NPCType Type) : IComponent;

/// <summary>
/// Specifies the role of an <see cref="NPC"/>.
/// </summary>
public enum NPCRole
{
    Merchant,
    ClientMenu,
    QuestNpc,
    StoreManager,
    NPCMenu,
    Gatenpc,
    Gate,
    IDGate,
    JobMaster,
    Guard,
    ModeIDGate,
    RandomGate
}

/// <summary>
/// Specifies the type of an <see cref="NPC"/>.
/// </summary>
public enum NPCType
{
    Weapon,
    SoulStone,
    WeaponTitle,
    Quest,
    Skill,
    Item,
    Guild,
    Gate,
    Job,
    ExchangeCoin,
    GBDice,
    RandomOption,
    Other
}