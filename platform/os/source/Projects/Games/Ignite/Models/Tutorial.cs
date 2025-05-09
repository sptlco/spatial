// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;

namespace Ignite.Models;

/// <summary>
/// An character's tutorial.
/// </summary>
public class Tutorial
{
    /// <summary>
    /// The character's <see cref="Tutorial"/> state.
    /// </summary>
    public TUTORIAL_STATE State { get; set; } = TUTORIAL_STATE.TS_PROGRESS;

    /// <summary>
    /// The character's <see cref="Tutorial"/> step.
    /// </summary>
    public byte Step { get; set; } = 0;
}