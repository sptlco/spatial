// Copyright © Spatial. All rights reserved.

using Spatial.Persistence;

namespace Ignite.Models;

/// <summary>
/// A persistent, player-owned entity.
/// </summary>
[DocumentCollection(Collection.Skills)]
public class Skill : Document
{
    /// <summary>
    /// The entity that learned the <see cref="Skill"/>.
    /// </summary>
    public uint Owner { get; set; }

    /// <summary>
    /// The skill's parent skill identification number.
    /// </summary>
    public ushort Parent { get; set; }
}