// Copyright © Spatial. All rights reserved.

using Spatial.Mathematics;
using System;

namespace Ignite.Models;

/// <summary>
/// An character's active kingdom quest.
/// </summary>
public class KingdomQuest
{
    /// <summary>
    /// The kingdom quest's handle.
    /// </summary>
    public uint Handle { get; set; }

    /// <summary>
    /// The map the <see cref="KingdomQuest"/> is taking place in.
    /// </summary>
    public string Map { get; set; }

    /// <summary>
    /// The character's position within the <see cref="KingdomQuest"/>.
    /// </summary>
    public Point2D Position { get; set; }

    /// <summary>
    /// The time the <see cref="KingdomQuest"/> began.
    /// </summary>
    public DateTime Time { get; set; }
}