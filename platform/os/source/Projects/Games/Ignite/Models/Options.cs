// Copyright © Spatial. All rights reserved.

namespace Ignite.Models;

/// <summary>
/// Configurable options for a <see cref="Character"/>.
/// </summary>
public class Options
{
    /// <summary>
    /// The character's shortcut options.
    /// </summary>
    public byte[] Shortcuts { get; set; } = [];

    /// <summary>
    /// The character's shortcut size options.
    /// </summary>
    public byte[] ShortcutSize { get; set; } = [];

    /// <summary>
    /// The character's sound options.
    /// </summary>
    public byte[] Sound { get; set; } = [];

    /// <summary>
    /// The character's video options.
    /// </summary>
    public byte[] Video { get; set; } = [];

    /// <summary>
    /// The character's game options.
    /// </summary>
    public byte[] Game { get; set; } = [];

    /// <summary>
    /// The character's window options.
    /// </summary>
    public byte[] Windows { get; set; } = [];

    /// <summary>
    /// The character's key mappings.
    /// </summary>
    public byte[] Keys { get; set; } = [];
}