// Copyright © Spatial. All rights reserved.

namespace Ignite.Models;

/// <summary>
/// A character's core stat distribution.
/// </summary>
public class Archetype {
    /// <summary>
    /// The character's strength points.
    /// </summary>
    public byte Strength { get; set; }

    /// <summary>
    /// The character's constitute points.
    /// </summary>
    public byte Endurance { get; set; }

    /// <summary>
    /// The character's dexterity points.
    /// </summary>
    public byte Dexterity { get; set; }

    /// <summary>
    /// The character's intelligence points.
    /// </summary>
    public byte Intelligence { get; set; }

    /// <summary>
    /// The character's wisdom points.
    /// </summary>
    public byte Wisdom { get; set; }

    /// <summary>
    /// The character's spirit points.
    /// </summary>
    public byte Spirit { get; set; }
    
    /// <summary>
    /// The character's stat point count.
    /// </summary>
    public byte Points { get; set; }
}